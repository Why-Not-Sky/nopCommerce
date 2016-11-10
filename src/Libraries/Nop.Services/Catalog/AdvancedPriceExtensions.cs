using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;

namespace Nop.Services.Catalog
{
    /// <summary>
    /// Advanced price extensions
    /// </summary>
    public static class AdvancedPriceExtensions
    {
        /// <summary>
        /// Filter advanced prices by a store
        /// </summary>
        /// <param name="source">Advanced prices</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Filtered advanced prices</returns>
        public static IEnumerable<AdvancedPrice> FilterByStore(this IEnumerable<AdvancedPrice> source, int storeId)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Where(advancedPrice => advancedPrice.StoreId == 0 || advancedPrice.StoreId == storeId);
        }

        /// <summary>
        /// Filter advanced prices for a customer
        /// </summary>
        /// <param name="source">Advanced prices</param>
        /// <param name="customer">Customer</param>
        /// <returns>Filtered advanced prices</returns>
        public static IEnumerable<AdvancedPrice> FilterForCustomer(this IEnumerable<AdvancedPrice> source, Customer customer)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (customer == null)
                return source.Where(advancedPrice => advancedPrice.CustomerRole == null);

            return source.Where(advancedPrice => advancedPrice.CustomerRole == null ||
                customer.CustomerRoles.Where(role => role.Active).Select(role => role.Id).Contains(advancedPrice.CustomerRole.Id));
        }

        /// <summary>
        /// Remove duplicated quantities (leave only an advanced price with minimum price)
        /// </summary>
        /// <param name="source">Advanced prices</param>
        /// <returns>Filtered advanced prices</returns>
        public static IEnumerable<AdvancedPrice> RemoveDuplicatedQuantities(this IEnumerable<AdvancedPrice> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            //get group of advanced prices with the same quantity
            var advancedPricesWithDuplicates = source.GroupBy(advancedPrice => advancedPrice.Quantity).Where(group => group.Count() > 1);

            //get advanced prices with higher prices 
            var duplicatedPrices = advancedPricesWithDuplicates.SelectMany(group =>
            {
                //find minimal price for quantity
                var minAdvancedPrice = group.Aggregate((currentMinAdvancedPrice, nextAdvancedPrice) =>
                    (currentMinAdvancedPrice.Price < nextAdvancedPrice.Price ? currentMinAdvancedPrice : nextAdvancedPrice));

                //and return all other with higher price
                return group.Where(advancedPrice => advancedPrice.Id != minAdvancedPrice.Id);
            });

            //return advanced prices without duplicates
            return source.Where(advancedPrice => !duplicatedPrices.Any(duplicatedPrice => duplicatedPrice.Id == advancedPrice.Id));
        }

        /// <summary>
        /// Filter advanced prices by date
        /// </summary>
        /// <param name="source">Advanced prices</param>
        /// <param name="date">Date in UTC; pass null to filter by current date</param>
        /// <returns>Filtered advanced prices</returns>
        public static IEnumerable<AdvancedPrice> FilterByDate(this IEnumerable<AdvancedPrice> source, DateTime? date = null)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (!date.HasValue)
                date = DateTime.UtcNow;

            return source.Where(advancedPrice =>
                (!advancedPrice.StartDateTimeUtc.HasValue || advancedPrice.StartDateTimeUtc.Value < date) &&
                (!advancedPrice.EndDateTimeUtc.HasValue || advancedPrice.EndDateTimeUtc.Value > date));
        }
    }
}
