using System;
using System.Collections.Generic;
using System.Security.Claims;
using DogGo.Models;
using DogGo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DogGo.Controllers
{
    public class DogsController : Controller
    {
        private readonly IDogRepository _dogRepo;

        // The constructor accepts an IConfiguration object as a parameter. 
        //This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public DogsController(IDogRepository dogRepository)
        {
            _dogRepo = dogRepository;
        }

        //GET USER ID from Cookie using find first value and looking for a claim type with the Name Identifier property
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
           
            return int.Parse(id);
            
        }

        // GET: Dogs
        //ActionResult is MVC thing, it created the index for us
        //we declared we wanted to enstaniate list in this method that contains all walkers linked to the walkers table that is accessed through the walkers Repository
        //returns a view result and passed in walkers, need to create a walkers view, and Razor does it for us
        [Authorize]
        public ActionResult Index()
        {
            int ownerId = GetCurrentUserId();
            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(ownerId);

            return View(dogs);
        }


        // GET: Dog/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);

            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // GET: DogController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: DogController/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dog dog)
        {
            try
            {
                // update the dogs OwnerId to the current user's Id AFTER THE DOG WAS CREATION WAS STARTED ON THE FORM
                dog.OwnerId = GetCurrentUserId();

                //add dog to database (including OwnerId assigned above)
                _dogRepo.AddDog(dog);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }

        // GET: Dog/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            int ownerId = GetCurrentUserId();
            Dog dog = _dogRepo.GetDogById(id);

            if (dog == null || dog.OwnerId != ownerId )
            {
                return NotFound();
            }

            return View(dog);
        }

        // POST: Owners/Edit/5
        //edit is returning Dog dog as a paramater to updateDog when you hit submit, 
        //delete is only returnign the id to deleteDog.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Dog dog)
        {
            int ownerId = GetCurrentUserId();

            try
            {
                if (dog == null || dog.OwnerId != ownerId)
                {
                    return NotFound();
                }
                else
                {
                    _dogRepo.UpdateDog(dog);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Dogs/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            int ownerId = GetCurrentUserId();
            Dog dog = _dogRepo.GetDogById(id);
            if (dog == null || dog.OwnerId != ownerId)
            {
                return NotFound();
            }

            return View(dog);
        }

        // POST: Dogs/Delete/5
        //Needed to compare dog.ownerId to cookies but GET Request for delete was only returing Id????
        //brought in getDogId since we are only sending the id to the delete function 

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Dog dog)
        {
            int ownerId = GetCurrentUserId();
            Dog deleteDog = _dogRepo.GetDogById(id);

            try
            {
                if (deleteDog.OwnerId != ownerId)
                {
                    return NotFound();
                }
                else
                {
                    _dogRepo.DeleteDog(id);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }

    }
}
