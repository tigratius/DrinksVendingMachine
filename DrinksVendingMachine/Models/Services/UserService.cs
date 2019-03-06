using System;
using System.Linq;
using DrinksVendingMachine.Models.Classes;
using DrinksVendingMachine.Models.Classes.json;
using DrinksVendingMachine.Models.Classes.Utils;
using DrinksVendingMachine.Models.Core;
using DrinksVendingMachine.Models.Entities;
using DrinksVendingMachine.Models.Interfaces;

namespace DrinksVendingMachine.Models.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<CoinEntity> _coinRepository;
        private readonly IRepository<DrinkEntity> _drinkRepository;
        private readonly ISimpleRepo<CurrentStateEntity> _stateRepository;
        private readonly IVendingMachineLogger _logger;

        private readonly VengineMachine _vengineMachine;
        private readonly Printer _printer;

        public UserService(IRepository<CoinEntity> coinRepository,
                            IRepository<DrinkEntity> drinkRepository,
                            ISimpleRepo<CurrentStateEntity> stateRepository,
                            IVendingMachineLogger logger)
        {
            _coinRepository = coinRepository;
            _drinkRepository = drinkRepository;
            _stateRepository = stateRepository;
            _logger = logger;
            _vengineMachine = new VengineMachine();
            _printer = new Printer(logger);
        }

        public int AddCoin(Guid id)
        {
            CoinEntity coinEntity = _coinRepository.Get(id);
            CurrentStateEntity currentState = _stateRepository.GetFirst();

            _logger.Info(" before add coin");
            _printer.CoinAndStateInfo(coinEntity, currentState);

            _vengineMachine.AddCoin(coinEntity, currentState);

            _logger.Info(" after add coin");
            _printer.CoinAndStateInfo(coinEntity, currentState);

            _logger.Info(" before _db.SaveChanges()...");
            _coinRepository.SaveChanges();

            return currentState.Deposit;
        }

        public DrinkOperationInfo BuyDrink(Guid id)
        {
            DrinkEntity drink = _drinkRepository.Get(id);
            CurrentStateEntity currentState = _stateRepository.GetFirst();
            _logger.Info(" before buy drink");
            _printer.DrinkAndStateInfo(drink, currentState);

            bool success = true;
            var msg = "";

            if (!_vengineMachine.BuyDrink(drink, currentState, _coinRepository.Queryable().ToList()))
            {
                success = false;
                msg =
                    " Невозможно совершить покупку! В автомате нет сдачи! Деньги возвращены полностью в качестве сдачи";
                _logger.Warning(msg);
            }

            _logger.Info(" after buy drink");
            _printer.DrinkAndStateInfo(drink, currentState);

            _logger.Info(" before _db.SaveChanges()...");
            _drinkRepository.SaveChanges();

            return new DrinkOperationInfo { Drink = drink, Change = currentState.Change, Success = success, Msg = msg };
        }

        public JsonBase IsCanBuy(Guid id)
        {
            DrinkEntity drink = _drinkRepository.Get(id);
            CurrentStateEntity currentState = _stateRepository.GetFirst();

            _printer.DrinkAndStateInfo(drink, currentState);

            if (!_vengineMachine.IsCanBuy(drink, currentState))
            {
                var msg = " Сумма недостаточна для покупки!";
                _logger.Warning(msg);
                return new JsonError(msg);
            }

            return new JsonSuccess();
        }

        public void GetChange()
        {
            CurrentStateEntity currentState = _stateRepository.GetFirst();
            _vengineMachine.GetChange(currentState, _coinRepository.Queryable().ToList());

            _logger.Info(" before _db.SaveChanges()...");
            _stateRepository.SaveChanges();
        }

        public ModelView GetViewModel()
        {
            return new ModelView
            {
                Drinks = _drinkRepository.Queryable().OrderBy(d => d.Name).ToList(),
                Coins = _coinRepository.Queryable().OrderBy(c => c.Value).ToList(),
                Deposit = _stateRepository.GetFirst().Deposit,
                Change = _stateRepository.GetFirst().Change
            };
        }
    }
}