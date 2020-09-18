using System.Collections.Generic;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DogGo.Controllers
{
    public class WalkersController : Controller
    {
        private readonly IWalkerRepository _walkerRepo;
        private readonly IWalksRepository _walksRepo;
        private readonly IOwnerRepository _ownerRepo;
        private readonly IDogRepository _dogRepo;




        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        // The constructor accepts an IConfiguration object as a parameter. 
        //This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalkersController(
            IOwnerRepository ownerRepository,
            IDogRepository dogRepository,
            IWalkerRepository walkerRepository,
            IWalksRepository walksRepository)
        {
            _ownerRepo = ownerRepository;
            _dogRepo = dogRepository;
            _walkerRepo = walkerRepository;
            _walksRepo = walksRepository;

        }


        // GET: Walkers
        //ActionResult is MVC thing, it created the index for us
        //we declared we wanted to enstaniate list in this method that contains all walkers linked to the walkers table that is accessed through the walkers Repository
        //returns a view result and passed in walkers, need to create a walkers view, and Razor does it for us
        //INDEX is the default path wen going to the view for this controller
        public ActionResult Index()
        {

            List<Walker> walkers = _walkerRepo.GetAllWalkers();

            return View(walkers);
        }

        // GET: Walkers/Details/5
         public ActionResult Details(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);
            List<Walks> walks = _walksRepo.GetWalksByWalkerId(id);
//            Owner owner = _walksRepo.GetOwnerByDogId(id);


            WalkerProfileViewModel vm = new WalkerProfileViewModel()
            {
                GetWalksByWalkerId = walks,
                Walker = walker,
  //              Owner = owner
                
            };

            return View(vm);
        }
        //public ActionResult Details(int id)
        //{
        //    Walker walker = _walkerRepo.GetWalkerById(id);

        //    if (walker == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(walker);
        //}

        // GET: WalkersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WalkersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalkersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalkersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalkersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalkersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
