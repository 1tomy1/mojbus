﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MojBus.Data;
using MojBus.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MojBus.ViewComponents
{
    public class StopsViewComponent : ViewComponent
    {
        MojBusContext _context;

        public StopsViewComponent(MojBusContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Gtfsstops> stops = await _context.Gtfsstops.ToListAsync();
            return View(stops);
        }
    }
}