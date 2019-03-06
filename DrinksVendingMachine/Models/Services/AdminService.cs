using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrinksVendingMachine.Models.Classes;
using DrinksVendingMachine.Models.Classes.Importers;
using DrinksVendingMachine.Models.Classes.Utils;
using DrinksVendingMachine.Models.Classes.Utils.Validating;
using DrinksVendingMachine.Models.Core;
using DrinksVendingMachine.Models.Entities;
using DrinksVendingMachine.Models.Interfaces;

namespace DrinksVendingMachine.Models.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepository<CoinEntity> _coinRepository;
        private readonly IRepository<DrinkEntity> _drinkRepository;
        private readonly IVendingMachineLogger _logger;

        private readonly VengineMachine _vengineMachine;
        private readonly Printer _printer;
        private readonly FileContext _fileContext;

        public AdminService(IRepository<CoinEntity> coinRepository,
            IRepository<DrinkEntity> drinkRepository,
            IVendingMachineLogger logger, IStrategy strategyImport)
        {
            _coinRepository = coinRepository;
            _drinkRepository = drinkRepository;
            _logger = logger;


            _vengineMachine = new VengineMachine();
            _fileContext = new FileContext(strategyImport);
            _printer = new Printer(logger);
        }

        public ModelView GetViewModel()
        {
            return new ModelView
            {
                Drinks = _drinkRepository.Queryable().OrderBy(d => d.Name).ToList(),
                Coins = _coinRepository.Queryable().OrderBy(c => c.Value).ToList(),
                Token = ValidatingTokens.GetToken()
            };
        }

        public DrinkOperationInfo AddDrink(string name, int cost, int count, string imgPath)
        {
            DrinkEntity drinkEntity = new DrinkEntity
            {
                Id = Guid.NewGuid(),
                Name = name,
                Count = count,
                ImagePath = imgPath,
                CostPrice = cost
            };

            _drinkRepository.Add(drinkEntity);

            _logger.Info(" before _db.SaveChanges()...");
            _drinkRepository.SaveChanges();

            /*new { success = true, message = "ok", id = drinkEntity.Id, name, cost, count, path = imgPath }*/

            return new DrinkOperationInfo
            {
                Drink = drinkEntity,
                Msg = "ok",
                Success = true
            };
        }

        public void RemoveDrink(Guid id)
        {
            _drinkRepository.Remove(_drinkRepository.Get(id));
            
            _logger.Info(" before _db.SaveChanges()...");
            _drinkRepository.SaveChanges();
        }

        public void ChangeDrinkName(Guid id, string name)
        {
            DrinkEntity drinkEntity = _drinkRepository.Get(id);
            _printer.DrinkInfo(drinkEntity);

            _vengineMachine.ChangeName(drinkEntity, name);

            _logger.Info(" before _db.SaveChanges()...");
            _drinkRepository.SaveChanges();
        }

        public void ChangeDrinkCount(Guid id, int count)
        {
            DrinkEntity drinkEntity = _drinkRepository.Get(id);
            _printer.DrinkInfo(drinkEntity);

            _vengineMachine.ChangeCount(drinkEntity, count);

            _logger.Info(" before _db.SaveChanges()...");
            _drinkRepository.SaveChanges();
        }

        public void ChangeDrinkCost(Guid id, int cost)
        {
            DrinkEntity drinkEntity = _drinkRepository.Get(id);
            _printer.DrinkInfo(drinkEntity);

            _vengineMachine.ChangeCost(drinkEntity, cost);

            _logger.Info(" before _db.SaveChanges()...");
            _drinkRepository.SaveChanges();
        }

        public void ChangeDrinkImage(Guid id, string path)
        {
            DrinkEntity drinkEntity = _drinkRepository.Get(id);
            _printer.DrinkInfo(drinkEntity);

            _vengineMachine.ChangeImage(drinkEntity, path);

            _logger.Info(" before _db.SaveChanges()...");
            _drinkRepository.SaveChanges();
        }

        public void ChangeCoinCount(Guid id, int count)
        {
            CoinEntity coinEntity = _coinRepository.Get(id);
            _printer.CoinInfo(coinEntity);

            _vengineMachine.ChangeCoinCount(coinEntity, count);

            _logger.Info(" before _db.SaveChanges()...");
            _drinkRepository.SaveChanges();
        }

        public void ChangeBlocking(Guid id, bool isBlocking)
        {
            CoinEntity coinEntity = _coinRepository.Get(id);
            _printer.CoinInfo(coinEntity);

            if (isBlocking)
            {
                _vengineMachine.Block(coinEntity);
            }
            else
            {
                _vengineMachine.UnBlock(coinEntity);
            }

            _logger.Info(" before _db.SaveChanges()...");
            _drinkRepository.SaveChanges();
        }


        public string Import(HttpPostedFileBase upload, string imagePath)
        {
            _logger.Info(" upload.FileName = " + upload.FileName);

            var ext = System.IO.Path.GetExtension(upload.FileName);

            if (_fileContext.IsAllowedExtension(ext))
            {
                _logger.Info(" import started...");
                List<Drink> drinks = null;
                try
                {
                    drinks = _fileContext.Import(upload.InputStream);
                }
                catch (Exception e)
                {
                    _logger.Error(" Неправильная структура файла!");
                    ExceptionWriter.WriteErrorDetailed(_logger, e);
                }

                _logger.Info(" import ended...");

                if (drinks != null)
                {
                    foreach (var drink in drinks)
                    {

                        //Если по каким-то причинам при импорте не нашлась картинка по указанному пути берем дефолтную
                        string path = GetPathDefaultImg(imagePath);
                        try
                        {
                            //Сохраняем файл в хранилище
                            path = SaveFile(drink.ImgPath, imagePath);
                        }
                        catch (Exception e)
                        {
                            _logger.Error(" Указан неверный путь к файлу!");
                            ExceptionWriter.WriteErrorDetailed(_logger, e);
                        }

                        DrinkEntity drinkEntity = new DrinkEntity
                        {
                            Id = Guid.NewGuid(),
                            ImagePath = path,
                            Name = drink.Name,
                            Count = drink.Count,
                            CostPrice = drink.Cost
                        };

                        _printer.DrinkInfo(drinkEntity);
                        _drinkRepository.Add(drinkEntity);
                    }

                    _logger.Info(" before _db.SaveChanges()...");
                    _drinkRepository.SaveChanges();
                }
                else
                {
                    return " Пустые данные!";
                }
            }
            else
            {
                return " Файл имеет неверный формат!";
            }

            return "";
        }

        private string SaveFile(string fileSource, string imageStoragePath)
        {
            _logger.Info(" SaveFile() called");
            _logger.Info(" fileSource = " + fileSource);
            var fileName = System.IO.Path.GetFileName(fileSource);
            var path = imageStoragePath + fileName;

            string destinationPath = HttpContext.Current.Server.MapPath(path);

            System.IO.File.Copy(fileSource, destinationPath, true);
            return path;
        }

        private string GetPathDefaultImg(string imageStoragePath)
        {
            return imageStoragePath + "default/def.jpg";
        }
    }
}