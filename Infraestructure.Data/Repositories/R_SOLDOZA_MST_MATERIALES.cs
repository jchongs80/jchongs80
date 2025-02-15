﻿using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data.Model;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Data.Repositories
{
    public class R_SOLDOZA_MST_MATERIALES : ISOLDOZA_MST_MATERIALES
    {
        private readonly ApplicationDbContext _context;
        public R_SOLDOZA_MST_MATERIALES(ApplicationDbContext context)
        {
            _context = context;
        }

        private string sql;
        public Task<IEnumerable<SOLDOZA_MST_MATERIALES>> GetAll()
        {
            var materiales = _context.materiales;
            return Task.FromResult(materiales.AsEnumerable<SOLDOZA_MST_MATERIALES>());
        }

       
        public bool Insert(SOLDOZA_MST_MATERIALES material)
        {
            try
            {
                if (material == null)
                {
                    return false;
                }

                _context.Add(material);
                _context.SaveChanges(true);
                return true;


            }
            catch
            {
                return false;
            }
        }

        public bool Update(SOLDOZA_MST_MATERIALES material)
        {
            try
            {
                if (material == null)
                {
                    return false;
                }

                _context.Update(material);
                _context.SaveChanges(true);
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
