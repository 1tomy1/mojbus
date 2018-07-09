﻿using Microsoft.AspNetCore.Mvc;
using MojBus.Data;
using MojBus.Extensions;
using MojBus.Models.FavouriteStops;
using System;

namespace MojBus.Controllers
{
    [Produces("application/json")]
    [Route("api/[action]")]
    public class MojBusApiController : Controller
    {
        MojBusContext _context;

        public MojBusApiController(MojBusContext context)
        {
            _context = context;
        }

        public IActionResult Stops()
        {
            return Json(_context.GetStops());
        }

        public IActionResult Routes()
        {
            return Json(_context.GetRoutes());
        }

        [HttpGet]
        public IActionResult StopData(string stopName, DateTime date)
        {
            return Json(_context.StopTimesForStop(stopName, date));
        }

        [HttpGet]
        public IActionResult StopDataForTripDirection(string stopName, int directionId, DateTime date)
        {
            return Json(_context.StopTimesForStop(stopName, directionId, date));
        }

        [HttpGet]
        public IActionResult StopDataForRoute(string stopName, string routeShortName, DateTime date)
        {
            return Json(_context.StopTimesForStop(stopName, routeShortName, date));
        }

        [HttpGet]
        public IActionResult StopDataForRouteTrip(string stopName, string routeShortName, string tripHeadSign, DateTime date)
        {
            return Json(_context.StopTimesForStop(stopName, routeShortName, tripHeadSign, date));
        }

        //new api methods
        [HttpGet]
        public IActionResult RoutesWithDepartureTimesForStop(string stopName, int directionId, DateTime date)
        {
            return Json(_context.StopTimesForStop(stopName, directionId, date));
        }

        [HttpGet]
        public IActionResult RouteDepartureTimesForStop(string stopName, string routeShortName, int directionId, DateTime date)
        {
            return Json(_context.StopTimesForStop(stopName, routeShortName, directionId, date));
        }

        [HttpGet]
        public IActionResult StopDataLoggedIn(string stopName, int directionId, string userId, DateTime date)
        {
            return Json(_context.StopTimesForStopLoggedIn(stopName, directionId, userId, date));
        }

        [HttpGet]
        public IActionResult GetFavouriteStopRouteData(string userId)
        {
            return Json(_context.GetFavouriteStopsLoggedIn(userId));
        }

        [HttpPost]
        public IActionResult AddStopRouteToFavourites(FavouriteStopRouteModel favouriteStop)
        {
            return Json(_context.AddStopRouteToFavourites(favouriteStop));
        }        
    }
}