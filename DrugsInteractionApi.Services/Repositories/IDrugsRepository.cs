using DrugsInteractionApi.Services.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugsInteractionApi.Services.Repositories
{
    public interface IDrugsRepository
    {
        /// <summary>
        /// Method to find interaction between drugs in database, order of arguments does not matter
        /// </summary>
        /// <param name="firstDrugName">Name of the first drug (in En, UK, PL or Ru language)</param>
        /// <param name="secondDrugName">Name of the second drug (in En, UK, PL or Ru language)</param>
        /// <returns>IList of <see cref="Task{IList{ResponsePart}}"> that contains data about interaction from different sources</returns>
        /// <remarks>
        /// If there is no interaction in db, returns empty list.
        /// Articles are not translated and will be in original language.
        /// </remarks>
        public Task<IList<ResponsePart>> SearchInteractionAsync(string firstDrugName, string secondDrugName);


        /// <summary>
        /// Method to find durgs with similar names
        /// </summary>
        /// <param name="drugName">Name or Part of Drug to search</param>
        /// <returns>List of drug responses that matches with given name</returns>
        public Task<IList<IList<string>>> FindDrugNamesWithSimilarNameAsync(string drugName);


        /// <summary>
        /// Method to save messages about not found drug interactions
        /// </summary>
        /// <param name="firstDrugName">Name of the first drug</param>
        /// <param name="secondDrugName">Name of the second drug</param>
        /// <returns></returns>
        public Task SaveNotFoundMessageAsync(string firstDrugName, string secondDrugName);

        /// <summary>
        /// Method to find drug interaction with following food/substances:
        /// food, caffeine, alcohol
        /// </summary>
        /// <param name="DrugName">Name of the Drug to search interaction with (in En, UK, PL or Ru language)</param>
        /// <returns> <see cref="ILookup{string, ResponsePart}"/> where key is substance name, and value is <see cref="ResponsePart"/>  that contains data about interaction and source</returns>
        public Task<ILookup<string?, ResponsePart>> SearchFoodInteractionAsync(string DrugName);
    }
}
