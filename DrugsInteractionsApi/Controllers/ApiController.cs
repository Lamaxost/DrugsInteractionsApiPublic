using DrugsInteractionApi.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DrugsInteractionsApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiController : Controller
    {
        private readonly IDrugsRepository _repository;
        private readonly ILogger<ApiController> _log;

        public ApiController(IDrugsRepository repository, ILogger<ApiController> log)
        {
            _repository = repository;
            _log = log;
        }


        [HttpGet("foodInteractions")]
        public async Task<IActionResult> Interactions(string drugName, bool? isSecondCall)
        {
            try
            {
                var drugs = drugName.Split(",");

                
                if (Array.TrueForAll(drugs, d => String.IsNullOrWhiteSpace(d)))
                {
                    return BadRequest("Drug Names must be provided and not empty or whitespace");
                }


                if (drugs.Length == 1)
                {
                    var foodInteractions = await _repository.SearchFoodInteractionAsync(drugs[0]);
                    var response = new
                    {
                        food = foodInteractions["food"],
                        caffeine = foodInteractions["caffeine"],
                        alcohol = foodInteractions["alcohol"],
                        medicines = foodInteractions["medicines"],
                    };
                    return Json(response);
                }
                else if (drugs.Length == 2)
                {
                    var firstDrugFoodInteractionsTask = _repository.SearchFoodInteractionAsync(drugs[0]);
                    var secondDrugFoodInteractionsTask =_repository.SearchFoodInteractionAsync(drugs[1]);
                    var medicinesInteractionTask = _repository.SearchInteractionAsync(drugs[0], drugs[1]);

                    var medicinesInteraction = await medicinesInteractionTask;

                    if (medicinesInteraction.Count == 0)
                    {
                        await _repository.SaveNotFoundMessageAsync(drugs[0], drugs[1]);
                    }

                    var firstDrugFoodInteractions = await firstDrugFoodInteractionsTask;
                    var secondDrugFoodInteractions = await secondDrugFoodInteractionsTask;

                    var response = new
                    {
                        food = firstDrugFoodInteractions["food"].Concat(secondDrugFoodInteractions["food"]),
                        caffeine = firstDrugFoodInteractions["caffeine"].Concat(secondDrugFoodInteractions["caffeine"]),
                        alcohol = firstDrugFoodInteractions["alcohol"].Concat(secondDrugFoodInteractions["alcohol"]),
                        medicines = medicinesInteraction
                    };
                    return Json(response);
                }
                else
                {
                    return BadRequest("Drug Name must contain 1 or 2 drug names separated by comma (,)");
                }

            }
            catch( Exception e)
            {
                if (isSecondCall!= null)
                {
                    _log.LogError("Req error | {m} {trace}", e.Message, e.StackTrace);
                    return StatusCode(500);
                }
                else
                {
                    return await Interactions(drugName, true);
                }
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProduct(string drugName)
        {
            try 
            {
                var drugs =await  _repository.FindDrugNamesWithSimilarNameAsync(drugName);

                return Content(JsonConvert.SerializeObject(drugs), "application/json");
            }
            catch(Exception e)
            {
                _log.LogCritical("Req error | {m} {trace}", e.Message, e.StackTrace);
                return StatusCode(500);
            }
        }
    }
}
