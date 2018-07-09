﻿using Microsoft.EntityFrameworkCore;
using MojBus.Data;
using MojBus.Data.Entities;
using MojBus.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace MojBus.Extensions
{
    public static class MojBusContextExtensions
    {
        public static IQueryable<Gtfsstops> GetStops(this MojBusContext context)
        {
            return context.Gtfsstops.OrderBy(x => x.StopName).ThenBy(x => x.StopDirectionId);
        }

        public static List<StopDataModel> StopTimesForStop(this MojBusContext context, string stopName, int directionId, DateTime date)
        {
            //TODO: CHANGE DATE TO CURRENT DATE - data in DB not up to date yet
            object[] sqlParams = {
                new SqlParameter("@StopName", stopName),
                new SqlParameter("@DirectionId", directionId),
                new SqlParameter("@Date", "20180601")
            };

            List<StopDataEntity> data = context
                .Set<StopDataEntity>()
                .FromSql("exec dbo.TripsWithTimesForStationAndTripDirection @StopName, @DirectionId, @Date;", sqlParams)
                .ToList();

            return data
                .StopDataEntityToModel(stopName)
                .GroupByTripShortName()
                .OrderBy(x => x.TripShortName)
                .ToList();
        }

        public static List<StopDataModel> StopTimesForStop(this MojBusContext context, string stopName, string routeShortName, int directionId, DateTime date)
        {
            //TODO: CHANGE DATE TO CURRENT DATE - data in DB not up to date yet
            object[] sqlParams = {
                new SqlParameter("@StopName", stopName),
                new SqlParameter("@RouteShortName", routeShortName),
                new SqlParameter("@DirectionId", directionId),
                new SqlParameter("@Date", "20180601")
            };

            List<StopDataEntity> data = context
                .Set<StopDataEntity>()
                .FromSql("exec dbo.TripsWithTimesForStationAndRouteDirection @StopName, @RouteShortName, @DirectionId, @Date;", sqlParams)
                .AsNoTracking()
                .ToList();

            return data.StopDataEntityToModel(stopName);
        }

        public static List<StopDataModel> StopTimesForStopLoggedIn(this MojBusContext context, string stopName, int directionId, string userId, DateTime date)
        {
            //TODO: CHANGE DATE TO CURRENT DATE - data in DB not up to date yet
            object[] sqlParams = {
                new SqlParameter("@StopName", stopName),
                new SqlParameter("@DirectionId", directionId),
                new SqlParameter("@Date", "20180601")
            };

            List<FavouriteStopRoutes> favouriteStopRoutes = context
                .FavouriteStopRoutes
                .Where(x => x.UserId == userId 
                && x.StopName == stopName 
                && x.DirectionId == directionId)
                .ToList();
            List<StopDataEntity> data = context
                .Set<StopDataEntity>()
                .FromSql("exec dbo.TripsWithTimesForStationAndTripDirection @StopName, @DirectionId, @Date;", sqlParams)
                .ToList();

            return data
                .StopDataEntityToModel(stopName)
                .GroupByTripShortName()
                .OrderBy(x => x.TripShortName)
                .ToList()
                .AddFavouritesToStops(favouriteStopRoutes);
        }        

        public static List<StopDataModel> GetFavouriteStopsLoggedIn(this MojBusContext context, string userId)
        {
            List<FavouriteStopRoutes> favouriteStopRoutes = context
                .FavouriteStopRoutes
                .Where(x => x.UserId == userId)
                .ToList();
            List<StopDataModel> temp = new List<StopDataModel>();
            DateTime date = DateTime.Now;

            if (favouriteStopRoutes.Count() == 0)
                return temp;

            foreach (FavouriteStopRoutes route in favouriteStopRoutes)
            {
                temp.AddRange(context.StopTimesForStop(route.StopName, route.RouteShortName, route.DirectionId, date)
                    .GroupByTripShortName()
                    .ToList());
            }

            return temp;
        }

        //old api methods
        public static IQueryable<Gtfsroutes> GetRoutes(this MojBusContext context)
        {
            return context.Gtfsroutes.OrderBy(x => x.RouteShortName);
        }

        public static List<StopDataModel> StopTimesForStop(this MojBusContext context, string stopName, DateTime date)
        {
            //TODO: CHANGE DATE TO CURRENT DATE - data in DB not up to date yet
            object[] sqlParams = {
                new SqlParameter("@StopName", stopName),
                new SqlParameter("@Date", "20180601")
            };

            List<StopDataEntity> data = context.Set<StopDataEntity>().FromSql("exec dbo.TripsWithTimesForStation @StopName, @Date;", sqlParams).ToList();

            return data.StopDataEntityToModel(stopName);
        }

        public static List<StopDataModel> StopTimesForStop(this MojBusContext context, string stopName, string routeShortName, DateTime date)
        {
            //TODO: CHANGE DATE TO CURRENT DATE - data in DB not up to date yet
            object[] sqlParams = {
                new SqlParameter("@StopName", stopName),
                new SqlParameter("@RouteShortName", routeShortName),
                new SqlParameter("@Date", "20180601")
            };

            List<StopDataEntity> data = context.Set<StopDataEntity>().FromSql("exec dbo.TripsWithTimesForStationAndRoute @StopName, @RouteShortName, @Date;", sqlParams).ToList();

            return data.StopDataEntityToModel(stopName);
        }

        public static List<StopDataModel> StopTimesForStop(this MojBusContext context, string stopName, string routeShortName, string tripHeadSign, DateTime date)
        {
            //TODO: CHANGE DATE TO CURRENT DATE - data in DB not up to date yet
            object[] sqlParams = {
                new SqlParameter("@StopName", stopName),
                new SqlParameter("@RouteShortName", routeShortName),
                new SqlParameter("@TripHeadSign", tripHeadSign),
                new SqlParameter("@Date", "20180601")
            };

            List<StopDataEntity> data = context.Set<StopDataEntity>().FromSql("exec dbo.TripsWithTimesForStationAndRouteTrip @StopName, @RouteShortName, @TripHeadSign, @Date;", sqlParams).ToList();

            return data.StopDataEntityToModel(stopName);
        }
    }
}
