using cqrsplayground.shared;
using Refit;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace cqrsplayground.generator
{
    public class Asset
    {
        public Asset(string name, double price)
        {
            this.Name = name;
            this.Price = price;
        }

        public string Name { get; set; }

        public double Price { get; set; }
    }

    static class Program
    {
        private static Random _rand;

        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }


            var list = enumerable as IList<T> ?? enumerable.ToList();
            return list.Count == 0 ? default(T) : list[_rand.Next(0, list.Count)];
        }

        private static IEnumerable<string> _counterparties = new[]
     {
                  "Amundi Asset Management",
                  "Natixis Global Asset Management",
                  "AXA Investment Managers",
                  "BNP Paribas Investment Partners",
                  "La Banque Postale Asset Management"
                };

        private static IEnumerable<Asset> _assets =
         new[]
        {
                      new Asset("BHP BILLITON FIN CMS5 2076_04", 99.66),
                      new Asset("NATIONAL AUSTRALIA BANK 2.000 11/12/20", 105.95),
                      new Asset("ORIGIN ENERGY FINANCE LT 0 06/16/71", 102.0),
                      new Asset("ORIGIN ENERGY FINANCE 0 09/16/74", 88.54),
                      new Asset("IMMOFINANZ AG 4.250 03/08/18", 4.74),
                      new Asset("REPUBLIC OF AUSTRIA 3.200 02/20/17", 104.15),
                      new Asset("BELGIUM KINGDOM 4.250 03/28/41", 146.93),
                      new Asset("PETROBRAS GLOBAL FIN 4.2500 2023_10", 73.15),
                      new Asset("LABEYRIE FINE FOODS 5.625 03/15/21", 106.27),
                      new Asset("THOM EUROPE SAS 7.375 07/15/19", 105.59),
                      new Asset("GIE PSA TRESORERIE 6.000 09/19/33", 112.99),
                      new Asset("REMY COINTREAU SA 5.180 12/15/16", 104.52),
                      new Asset("TEREOS FINANCE GROUP I 4.250 03/04/20", 93.32),
                      new Asset("SMCP SAS 8.875 06/15/20", 107.37),
                      new Asset("MAGNOLIA BC SA 9.000 08/01/20", 108.06),
                      new Asset("PICARD GROUPE SA 0 08/01/19", 100.73),
                      new Asset("PEUGEOT SA 6.500 01/18/19", 115.58),
                      new Asset("RENAULT S.A. 3.625 09/19/18", 107.39),
                      new Asset("INFRA FOCH SAS 2.125 04/16/25", 100.44),
                      new Asset("NOVALIS 3.0000 2022_04", 98.62),
                      new Asset("PAPREC HOLDING 5.2500 2022_04", 100.37),
                      new Asset("LA FINAC ATALIAN SA 7.250 01/15/20", 108.6),
                      new Asset("KERNEOS TECH GROUP SAS 5.750 03/01/21", 102.53),
                      new Asset("DRY MIX SOLUTIONS INVEST 0 06/15/21", 97.92),
                      new Asset("CROWN EURO HOLDINGS SA 4.000 07/15/22", 104.49),
                      new Asset("AREVA SA 4.875 09/23/24", 104.53),
                      new Asset("SUEZ ENVIRONNEMENT +0bp 02/27/20", 22.58),
                      new Asset("AREVA SA 4.375 11/06/19", 106.79),
                      new Asset("AREVA SA 3.500 03/22/21", 101.48),
                      new Asset("AREVA SA 3.250 09/04/20", 100.91),
                      new Asset("AREVA 3.1250 2023_03", 96.49),
                      new Asset("GDF SUEZ 1.5000 2017_07", 102.11),
                      new Asset("VEOLIA ENVIRONNEMENT 0 01/29/49", 104.0),
                      new Asset("EVONIK / RAG-STIFTUNG 0% 12/2018", 108.99)
        };


        static void Main(string[] args)
        {
            var client = RestService.For<ITradeService>(ServiceConstants.TradeServiceUrl);

            _rand = new Random();

            var log = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            while (true)
            {
                Thread.Sleep(500);

                var asset = _assets.Random();

                var tradeCreation = new TradeCreationDemand()
                {
                    Asset = asset.Name,
                    Way = _rand.Next(0, 2) == 0 ? TradeWay.Sell : TradeWay.Buy,
                    Counterpary = _counterparties.Random(),
                    Currency = "EUR",
                    Nominal = _rand.Next(10, 100),
                    Price = asset.Price
                };


                log.Information($"{tradeCreation}");

               client.Create(tradeCreation);

            }
        }
    }
}
