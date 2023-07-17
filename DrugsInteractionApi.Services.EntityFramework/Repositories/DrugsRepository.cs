using DrugsInteractionApi.Services.Entities.DTO;
using DrugsInteractionApi.Services.EntityFramework.Entities.DrugsCom.NewBase;
using DrugsInteractionApi.Services.EntityFramework.Entities.DrugsCom.OldBase;
using DrugsInteractionApi.Services.EntityFramework.Entities.Ktomalek;
using DrugsInteractionApi.Services.EntityFramework.Entities.NotFound;
using DrugsInteractionApi.Services.Repositories;
using DrugsInteractionsApi.Services.EntityFramework.Entities.CheckMed;
using DrugsInteractionsApi.Services.EntityFramework.Entities.ComboMed;
using DrugsInteractionsApi.Services.EntityFramework.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

#pragma warning disable S3267
namespace DrugsInteractionApi.Services.EntityFramework.Repositories
{
    public class DrugsRepository : IDrugsRepository
    {
        private readonly IDbContextFactory<CombomedOldContext> _combomedOldContextFactory;
        private readonly IDbContextFactory<DrugsComContext> _drugscomOldContextFactory;
        private readonly IDbContextFactory<DrugsCom2Context> _drugscomNewContextFactory;
        private readonly IDbContextFactory<CheckMedContext> _checkmedContextFactory;
        private readonly IDbContextFactory<KtomalekContext> _ktomalekContextFactory;
        private readonly IDbContextFactory<NotFoundContext> _notFoundContextFactory;
        private readonly IDbContextFactory<CombomedNewContext> _combomedNewContextFactory;

        public DrugsRepository(IDbContextFactory<CombomedOldContext> combomedOldContextFactory, IDbContextFactory<DrugsComContext> drugscomOldContextFactory, IDbContextFactory<DrugsCom2Context> drugscomNewContextFactory, IDbContextFactory<CheckMedContext> checkmedContextFactory, IDbContextFactory<KtomalekContext> ktomalekContextFactory, IDbContextFactory<NotFoundContext> notFoundContextFactory, IDbContextFactory<CombomedNewContext> combomedNewContextFactory)
        {
            _combomedOldContextFactory = combomedOldContextFactory;
            _combomedNewContextFactory = combomedNewContextFactory;
            _drugscomOldContextFactory = drugscomOldContextFactory;
            _drugscomNewContextFactory = drugscomNewContextFactory;
            _checkmedContextFactory = checkmedContextFactory;
            _ktomalekContextFactory = ktomalekContextFactory;
            _notFoundContextFactory = notFoundContextFactory;
        }

#nullable disable
        public async Task<IList<IList<string>>> FindDrugNamesWithSimilarNameAsync(string drugName)
        {
            var result = new List<IList<string>>();
            using (var context = _combomedOldContextFactory.CreateDbContext())
            {
                var names = await context.Drugs.Where(d => Regex.IsMatch(d.NameUk, drugName) || Regex.IsMatch(d.NameRu, drugName) || Regex.IsMatch(d.NameUk, drugName)).Select(d => new List<string>() { d.NameRu, d.NameUk, d.NameEn }).ToListAsync();
                result.AddRange(names);
            }
            using (var context = _combomedNewContextFactory.CreateDbContext())
            {
                var names = await context.Drugs.Where(d => Regex.IsMatch(d.NameUk, drugName) || Regex.IsMatch(d.NameRu, drugName) || Regex.IsMatch(d.NameUk, drugName)).Select(d => new List<string>() { d.NameRu, d.NameUk, d.NameEn }).ToListAsync();
                result.AddRange(names);
            }
            using (var context = _drugscomNewContextFactory.CreateDbContext())
            {
                var names = await context.Drugs.Where(d => Regex.IsMatch(d.NameUk, drugName) || Regex.IsMatch(d.NameRu, drugName) || Regex.IsMatch(d.NameUk, drugName)).Select(d => new List<string>() { d.NameRu, d.NameUk, d.NameEn }).ToListAsync();
                result.AddRange(names);
            }
            using (var context = _drugscomNewContextFactory.CreateDbContext())
            {
                var names = await context.Drugs.Where(d => Regex.IsMatch(d.NameUk, drugName) || Regex.IsMatch(d.NameRu, drugName) || Regex.IsMatch(d.NameUk, drugName)).Select(d => new List<string>() { d.NameRu, d.NameUk, d.NameEn }).ToListAsync();
                result.AddRange(names);
            }
            using (var context = _ktomalekContextFactory.CreateDbContext())
            {
                var names = await context.Drugs.Where(d => Regex.IsMatch(d.NameUk, drugName) || Regex.IsMatch(d.NameRu, drugName) || Regex.IsMatch(d.NameUk, drugName)).Select(d => new List<string>() { d.NameRu, d.NameUk, d.NameEn }).ToListAsync();
                result.AddRange(names);
            }
            using (var context = _checkmedContextFactory.CreateDbContext())
            {
                var names = await context.Drugs.Where(d => Regex.IsMatch(d.NameUk, drugName) || Regex.IsMatch(d.NameRu, drugName) || Regex.IsMatch(d.NameUk, drugName)).Select(d => new List<string>() { d.NameRu, d.NameUk, d.NameEn }).ToListAsync();
                result.AddRange(names);
            }

            result = result.Select(r => r.Where(rn => !string.IsNullOrEmpty(rn)).ToList())
                .Where(r => r.Count > 0)
                .DistinctBy(d => d[0])
                .Cast<IList<string>>()
                .ToList();
            return result;
        }
#nullable enable
        public async Task SaveNotFoundMessageAsync(string firstDrugName, string secondDrugName)
        {
            var message = new NotFoundMessage()
            {
                Drug1Name = firstDrugName,
                Drug1Found = isDrugInDb(firstDrugName),
                Drug2Found = isDrugInDb(secondDrugName),
                Drug2Name = secondDrugName,
                InteractionFound = (await SearchInteractionAsync(firstDrugName, secondDrugName)).Count != 0
            };
            using (var context = _notFoundContextFactory.CreateDbContext())
            {
                context.Messages.Add(message);
                context.SaveChanges();
            }
        }

        public async Task<ILookup<string?, ResponsePart>> SearchFoodInteractionAsync(string DrugName)
        {
            var drugsResult = new List<ResponsePart>();

            var drugs = DrugName.Split(",");
            await Parallel.ForEachAsync(drugs, parallelOptions: new ParallelOptions() { MaxDegreeOfParallelism = 3 },body: async (drug,token) =>
            {
                var tasks = new List<Task<IList<ResponsePart>>>()
                {
                    SearchInteractionAsync(drug, "food"),
                    SearchInteractionAsync(drug, "caffeine"),
                    SearchInteractionAsync(drug, "alcohol"),
                    SearchInteractionAsync(drug, "ethanol"),
                };
                var taskResults = await Task.WhenAll(tasks);
                var drugResult = taskResults.SelectMany(t=>t).ToList();

                foreach (var r in drugResult)
                {
                    if (r.Answer.Drug2Name == "ethanol")
                    {
                        r.Answer.Drug2Name = "alcohol";
                    }

                }
                drugResult = drugResult.DistinctBy(d => d.Answer.Drug2Name + d.Source + d.Answer.Drug1Name).ToList();
                lock (drugsResult)
                {
                    drugsResult.AddRange(drugResult);
                }
            });

            var interactionsGroupedByFood = drugsResult.ToLookup(g => g.Answer.Drug2Name, g => g);

            foreach (var r in drugsResult)
            {
                if (r.Answer.Drug2Name == "alcohol")
                {
                    r.Answer.Drug2Name = "Алкоголь";
                }
                if (r.Answer.Drug2Name == "caffeine")
                {
                    r.Answer.Drug2Name = "Кофеїн";
                }
                if (r.Answer.Drug2Name == "food")
                {
                    r.Answer.Drug2Name = "Їжа";
                }
            }
            return interactionsGroupedByFood!;
        }

        public async Task<IList<ResponsePart>> SearchInteractionAsync(string firstDrugName, string secondDrugName)
        {
            var tasks = new Task<ResponsePart?>[]
            {
                GetCombomedInteraction(firstDrugName, secondDrugName),
                GetCheckMedInteraction(firstDrugName, secondDrugName),
                GetDrugsComInteraction(firstDrugName, secondDrugName),
                GetKtomalekInteraction(firstDrugName, secondDrugName),
            };
            var tasks_result = await Task.WhenAll(tasks);
            var result = tasks_result?.Where(r=>r != null).Cast<ResponsePart>().Where(r => !String.IsNullOrWhiteSpace(r.Answer.Article)).ToList();
            return result??new List<ResponsePart>();
        }

        private async Task<ResponsePart?> GetDrugsComInteraction(string firstDrugName, string secondDrugName)
        {
            var firstDrugNameSearchForm = firstDrugName.ToLower().Trim();
            var secondDrugNameSearchForm = secondDrugName.ToLower().Trim();

            DrugsInteractionRow? interaction = null;

            // firstly we check old database and only if there were no result, we check new

            using (var drugsContext = _drugscomOldContextFactory.CreateDbContext())
            {

                var drug1 = FindDrugByMultiLanguageName(drugsContext.Drugs, firstDrugNameSearchForm);
                var drug2 = FindDrugByMultiLanguageName(drugsContext.Drugs, secondDrugNameSearchForm);
                if (drug1 == null || drug2 == null)
                {
                    return null;
                }
                if (drug1.GenericIds?.Length > 0 || drug2.GenericIds?.Length > 0)
                {
                    var drug1Generics = drug1.GenericIds;
                    var drug2Generics = drug2.GenericIds;

                    if (drug1Generics == null)
                    {
                        drug1Generics = new int[] { drug1.Id };
                    }
                    if (drug2Generics == null)
                    {
                        drug2Generics = new int[] { drug2.Id };
                    }

                    var interactions = new List<DrugsInteractionRow>();

                    if (drug1Generics.Length == 1 && drug2Generics.Length == 1)
                    {

                        var genericsInteraction1Task = drugsContext.InteractionRows.FirstOrDefaultAsync(i => i.Drug1Id == drug1Generics[0] && i.Drug2Id == drug2Generics[0]);

                        var genericsInteraction = await genericsInteraction1Task;

                        if (genericsInteraction == null)
                        {
                            var genericsInteraction2Task = drugsContext.InteractionRows.FirstOrDefaultAsync(i => i.Drug1Id == drug2Generics[0] && i.Drug2Id == drug1Generics[0]);
                            genericsInteraction = await genericsInteraction2Task;
                        }
                        if (genericsInteraction != null)
                        {
                            interactions.Add(genericsInteraction);
                        }

                    }
                    foreach (var drug1Generic in drug1Generics)
                    {
                        foreach (var drug2Generic in drug2Generics)
                        {
                            var genericsInteraction1Task = drugsContext.InteractionRows.FirstOrDefaultAsync(i => i.Drug1Id == drug1Generic && i.Drug2Id == drug2Generic);
                            var genericsInteraction = await genericsInteraction1Task;
                            if (genericsInteraction == null)
                            {
                                var genericsInteraction2Task = drugsContext.InteractionRows.FirstOrDefaultAsync(i => i.Drug1Id == drug2Generic && i.Drug2Id == drug1Generic);
                                genericsInteraction = await genericsInteraction2Task;
                            }
                            if (genericsInteraction != null)
                            {
                                interactions.Add(genericsInteraction);
                            }
                        }
                    }

                    if (interactions.Count == 0)
                    {
                        return null;
                    }
                    interaction = new DrugsInteractionRow()
                    {
                        Drug1Id = drug1.Id,
                        Drug2Id = drug2.Id,
                        Article = String.Join("\n", interactions.Select(i => i.Article))
                    };

                    if (interaction != null)
                    {
                        var responseResult = new InteractionResponse()
                        {
                            Drug1Name = firstDrugName,
                            Drug2Name = secondDrugName,
                            Article = interaction.Article
                        };
                        var responsePart = new ResponsePart()
                        {
                            Answer = responseResult,
                            Source = "https://drugs.com",
                            Language = "en",
                        };
                        return responsePart;
                    }
                }
            }

            if (interaction == null)
            {
                using (var drugs2Context = _drugscomNewContextFactory.CreateDbContext())
                {
                    var drug1 = FindDrugByMultiLanguageName(drugs2Context.Drugs, firstDrugNameSearchForm);
                    var drug2 = FindDrugByMultiLanguageName(drugs2Context.Drugs, secondDrugNameSearchForm);
                    if (drug1 == null || drug2 == null)
                    {
                        return null;
                    }
                    var drug1Generics = new List<DrugsCom2Drug>() { drug1 };
                    if (drug1.GenericIds?.Count > 0)
                    {
                        for (int i = 0; i < drug1Generics.Count; i++)
                        {
                            var drug = drug1Generics[i];
                            var currentGenerics = drugs2Context.Drugs.Where(d => drug!.GenericIds!.Contains(d.Id)).ToArray();
                            drug1Generics.AddRange(currentGenerics);
                            drug1Generics = drug1Generics.DistinctBy(d => d.Id).ToList();

                        }
                    }
                    var drug2Generics = new List<DrugsCom2Drug>() { drug2 };
                    if (drug2.GenericIds?.Count > 0)
                    {
                        for (int i = 0; i < drug2Generics.Count; i++)
                        {
                            var drug = drug2Generics[i];
                            var currentGenerics = drugs2Context.Drugs.Where(d => drug!.GenericIds!.Contains(d.Id)).ToArray();
                            drug2Generics.AddRange(currentGenerics);
                            drug2Generics = drug2Generics.DistinctBy(d => d.Id).ToList();
                        }
                    }
                    drug1Generics = drug1Generics.Where(d => d.IsGeneric).ToList();
                    drug2Generics = drug2Generics.Where(d => d.IsGeneric).ToList();
                    var possibleInteractions = new List<int[]>();
                    foreach (var g1 in drug1Generics)
                    {
                        foreach (var g2 in drug2Generics)
                        {
                            var possibleInteraction = g1.Id > g2.Id ? new int[] { g1.Id, g2.Id } : new int[] { g2.Id, g1.Id };
                            possibleInteractions.Add(possibleInteraction);
                        }
                    }
                    foreach (var possibleInteraction in possibleInteractions)
                    {
                        var interactionResult = await drugs2Context.InteractionRows.FirstOrDefaultAsync(i => i.Drug1Id == possibleInteraction[0] && i.Drug2Id == possibleInteraction[1]);
                        if (interactionResult != null)
                        {
                            interactionResult.Article = interactionResult.Article.Replace("Information for this minor interaction is available on the professional version.\n", "");
                            var responseInteraction = new InteractionResponse()
                            {
                                Drug1Name = firstDrugName,
                                Drug2Name = secondDrugName,
                                Article = interactionResult.Article,
                            };
                            var responsePart = new ResponsePart()
                            {
                                Answer = responseInteraction,
                                Language = "en",
                                Source = "https://drugs.com",
                            };
                            return responsePart;
                        }
                    }

                }

            }

            return null;
        }

        private async Task<ResponsePart?> GetCheckMedInteraction(string firstDrugName, string secondDrugName)
        {
            CheckMedInteractionRow? interaction = null;
            using (var checkmedContext = _checkmedContextFactory.CreateDbContext())
            {
                var firstDrugNameSearchForm = firstDrugName.ToLower().Trim();
                var secondDrugNameSearchForm = secondDrugName.ToLower().Trim();
                Drug? drug1 = FindDrugByMultiLanguageName(checkmedContext.Drugs, firstDrugNameSearchForm);
                Drug? drug2 = FindDrugByMultiLanguageName(checkmedContext.Drugs, secondDrugNameSearchForm);
                if (drug1 == null || drug2 == null)
                {
                    return null;
                }
                var interactionTask = checkmedContext.InteractionRows.FirstOrDefaultAsync(i => i.Drug1Id == drug1.Id && i.Drug2Id == drug2.Id);
                interaction = await interactionTask;
                if (interaction == null)
                {
                    var interactionSwitchedNamesTask = checkmedContext.InteractionRows.FirstOrDefault(i => i.Drug1Id == drug2.Id && i.Drug2Id == drug1.Id);
                    interaction = interactionSwitchedNamesTask;
                }
                if (interaction != null)
                {
                    var checkMedResult = new ResponsePart();
                    var interactionResponse = new InteractionResponse()
                    {
                        Drug1Name = firstDrugName,
                        Drug2Name = secondDrugName,
                        Article = interaction.Article.Trim()
                    };
                    if (string.IsNullOrEmpty(interactionResponse.Article))
                    {
                        return null;
                    }
                    checkMedResult.Answer = interactionResponse;
                    checkMedResult.Source = "https://checkmed.info";
                    checkMedResult.Language = "ru";
                    return checkMedResult;
                }
            }
            return null;
        }

        private async Task<ResponsePart?> GetCombomedInteraction(string firstDrugName, string secondDrugName)
        {
            CombomedInteractionRow? interaction = null;
            var firstDrugNameSearchForm = firstDrugName.ToLower().Trim();
            var secondDrugNameSearchForm = secondDrugName.ToLower().Trim();

            // firstly we check old database and only if there were no result, we check new

            using (var combomedContext = _combomedOldContextFactory.CreateDbContext())
            {
                Drug? drug1 = FindDrugByMultiLanguageName(combomedContext.Drugs, firstDrugNameSearchForm);
                Drug? drug2 = FindDrugByMultiLanguageName(combomedContext.Drugs, secondDrugNameSearchForm);
                if (drug1 == null || drug2 == null)
                {
                    return null;
                }
                var interactionTask = combomedContext.InteractionRows.FirstOrDefaultAsync(i => i.Drug1Id == drug1.Id && i.Drug2Id == drug2.Id);

                interaction = await interactionTask;
                if (interaction == null)
                {
                    var interactionSwitchedNamesTask = combomedContext.InteractionRows.FirstOrDefaultAsync(i => i.Drug1Id == drug2.Id && i.Drug2Id == drug1.Id);
                    interaction = await interactionSwitchedNamesTask;
                }
                if (interaction != null)
                {
                    var combomedResult = new ResponsePart();
                    var interactionResponse = new InteractionResponse()
                    {
                        Drug1Name = firstDrugName,
                        Drug2Name = secondDrugName,
                        Article = (interaction.Article ?? "" + "\n" + interaction.Article2 ?? "").Trim()
                    };
                    if (string.IsNullOrEmpty(interactionResponse.Article))
                    {
                        return null;
                    }
                    combomedResult.Answer = interactionResponse;
                    combomedResult.Source = "https://combomed.ru";
                    combomedResult.Language = "ru";
                    return combomedResult;
                }
            }
            
            // if interaction is not found in old database

            if (interaction == null)
            {
                using (var combomedContext = _combomedNewContextFactory.CreateDbContext())
                {
                    Drug? drug1 = FindDrugByMultiLanguageName(combomedContext.Drugs, firstDrugNameSearchForm);
                    Drug? drug2 = FindDrugByMultiLanguageName(combomedContext.Drugs, secondDrugNameSearchForm);
                    if (drug1 == null || drug2 == null)
                    {
                        return null;
                    }
                    var interactionTask = combomedContext.InteractionRows.FirstOrDefaultAsync(i => i.Drug1Id == drug1.Id && i.Drug2Id == drug2.Id);

                    interaction = await interactionTask;
                    if (interaction == null)
                    {
                        var interactionSwitchedNamesTask = combomedContext.InteractionRows.FirstOrDefault(i => i.Drug1Id == drug2.Id && i.Drug2Id == drug1.Id);
                        interaction = interactionSwitchedNamesTask;
                    }
                    if (interaction != null)
                    {
                        var combomedResult = new ResponsePart();
                        var interactionResponse = new InteractionResponse()
                        {
                            Drug1Name = firstDrugName,
                            Drug2Name = secondDrugName,
                            Article = (interaction.Article ?? "" + "\n" + interaction.Article2 ?? "").Trim()
                        };
                        if (string.IsNullOrEmpty(interactionResponse.Article))
                        {
                            return null;
                        }
                        combomedResult.Answer = interactionResponse;
                        combomedResult.Source = "https://combomed.ru";
                        combomedResult.Language = "ru";
                        return combomedResult;
                    }
                }
            }
            return null;
        }

        private async Task<ResponsePart?> GetKtomalekInteraction(string firstDrugName, string secondDrugName)
        {
            var firstDrugNameSearchForm = firstDrugName.ToLower().Trim();
            var secondDrugNameSearchForm = secondDrugName.ToLower().Trim();

            using (var ktomalekContext = _ktomalekContextFactory.CreateDbContext())
            {
                if (secondDrugName == "food")
                {
                    var drug1 = FindDrugByMultiLanguageName(ktomalekContext.Drugs, firstDrugNameSearchForm).FirstOrDefault();
                    var drugs2 = FindDrugByMultiLanguageName(ktomalekContext.Drugs, secondDrugNameSearchForm).Select(d => d!.Id).ToList();
                    if (drug1 == null || drugs2.Count <= 0)
                    {
                        return null;
                    }
                    var interactions = ktomalekContext.InteractionRows.Where(i => drug1.Id == i.Drug1Id && drugs2.Contains(i.Drug2Id));

                    var articleIds = interactions.Select(i => i.ArticleId).ToList();

                    var articles = ktomalekContext.Articles.Where(a => articleIds.Contains(a.Id)).ToList();

                    if (string.IsNullOrWhiteSpace(String.Join("\n", articles.Select(i => i.Article)).Trim()))
                    {
                        return null;
                    }

                    var responseInteraction = new InteractionResponse()
                    {
                        Drug1Name = firstDrugName,
                        Drug2Name = secondDrugName,
                        Article = String.Join("\n", articles.Select(i => i.Article)).Trim()
                    };

                    var responsePart = new ResponsePart() { Answer = responseInteraction, Language = "pl", Source = "https://ktomalek.pl" };

                    return responsePart;
                }
                else
                {
                    var drug1Id = FindDrugByMultiLanguageName(ktomalekContext.Drugs, firstDrugNameSearchForm).FirstOrDefault()?.Id;
                    var drug2Id = FindDrugByMultiLanguageName(ktomalekContext.Drugs, secondDrugNameSearchForm).FirstOrDefault()?.Id;

                    if (drug1Id == null || drug2Id == null)
                    {
                        return null;
                    }
                    var ktomalekInteraction1Task = ktomalekContext.InteractionRows.FirstOrDefaultAsync(i => i.Drug1Id == drug1Id && i.Drug2Id == drug2Id);
                    var ktomalekInteraction = await ktomalekInteraction1Task;
                    if (ktomalekInteraction == null)
                    {
                        var ktomalekInteraction2Task = ktomalekContext.InteractionRows.FirstOrDefaultAsync(i => i.Drug1Id == drug2Id && i.Drug2Id == drug1Id);
                        ktomalekInteraction = await ktomalekInteraction2Task;
                    }
                    if (ktomalekInteraction == null)
                    {
                        return null;
                    }

                    var article = ktomalekContext.Articles.FirstOrDefault(a => a.Id == ktomalekInteraction.ArticleId);

                    if (article == null) return null;

                    ktomalekInteraction.Article = article;

                    var responseInteraction = new InteractionResponse
                    {
                        Drug1Name = firstDrugName,
                        Drug2Name = secondDrugName,
                        Article = article.Article
                    };

                    var responsePart = new ResponsePart() { Answer = responseInteraction, Language = "pl", Source = "https://ktomalek.pl" };
                    return responsePart;
                }
            }
        }

        private bool isDrugInDb(string name)
        {
            var drugNameSearchForm = name.ToLower().Trim();


            using (var combomedContext = _combomedNewContextFactory.CreateDbContext())
            {
                if (FindDrugByMultiLanguageName(combomedContext.Drugs, drugNameSearchForm) != null)
                {
                    return true;
                }
                else
                {
                    using (var combomed2Context = _combomedOldContextFactory.CreateDbContext())
                    {
                        if (FindDrugByMultiLanguageName(combomed2Context.Drugs, drugNameSearchForm) != null)
                        {
                            return true;
                        }
                    }
                }
            }
            using (var checkmedContext = _checkmedContextFactory.CreateDbContext())
            {
                if (FindDrugByMultiLanguageName(checkmedContext.Drugs, drugNameSearchForm) != null)
                {
                    return true;
                }
            }
            using (var drugscomContext = _drugscomOldContextFactory.CreateDbContext())
            {
                if (FindDrugByMultiLanguageName(drugscomContext.Drugs, drugNameSearchForm) != null)
                {
                    return true;
                }
            }
            using (var drugscomContext = _drugscomNewContextFactory.CreateDbContext())
            {
                if (FindDrugByMultiLanguageName(drugscomContext.Drugs, drugNameSearchForm) != null)
                {
                    return true;
                }
            }
            using (var ktomalekContext = _ktomalekContextFactory.CreateDbContext())
            {
                if (FindDrugByMultiLanguageName(ktomalekContext.Drugs, drugNameSearchForm).Count != 0)
                {
                    return true;
                }
            }
            return false;
        }

        private List<KtomalekDrug?> FindDrugByMultiLanguageName(DbSet<KtomalekDrug> drugs, string name)
        {
            var result = drugs.AsNoTracking().Where(drug => drug.NameRu == name || drug.NameEn == name || drug.NameUk == name || drug.NamePl == name).ToList();
            return result!;
        }

        private DrugsComDrug? FindDrugByMultiLanguageName(DbSet<DrugsComDrug> drugs, string name)
        {
            DrugsComDrug? drug = drugs.AsNoTracking().FirstOrDefault(drug => drug.NameRu == name || drug.NameEn == name || drug.NameUk == name);
            return drug;
        }

        private DrugsCom2Drug? FindDrugByMultiLanguageName(DbSet<DrugsCom2Drug> drugs, string name)
        {
            DrugsCom2Drug? drug = drugs.AsNoTracking().FirstOrDefault(drug => drug.NameRu == name || drug.NameEn == name || drug.NameUk == name);
            return drug;
        }

        private Drug? FindDrugByMultiLanguageName(DbSet<Drug> drugs, string name)
        {
            Drug? drug = drugs.AsNoTracking().FirstOrDefault(drug => drug.NameRu == name || drug.NameEn == name || drug.NameUk == name);
            return drug;
        }
    }
}
