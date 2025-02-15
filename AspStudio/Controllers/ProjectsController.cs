﻿using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspStudio.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ILogger<ProjectsController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ISOLDOZA_MST_GRL_PROYECTOS _proyectos;
        private readonly ISOLDOZA_MST_PLANOS _nplano;
        private readonly ISOLDOZA_MST_GRL_CLIENTES _clientes;
        private readonly ISOLDOZA_MST_GRL_INSTALACION _insta;
        private readonly ISOLDOZA_MST_GRL_INSTALACION_TIPO _instatipo;
        public ProjectsController(ILogger<ProjectsController> logger, ApplicationDbContext context,
                                    ISOLDOZA_MST_GRL_PROYECTOS proyectos, ISOLDOZA_MST_PLANOS nplano,
                                    ISOLDOZA_MST_GRL_CLIENTES clientes, ISOLDOZA_MST_GRL_INSTALACION_TIPO instatipo, ISOLDOZA_MST_GRL_INSTALACION insta)
        {
            _logger = logger;
            _context = context;
            _proyectos = proyectos;
            _nplano = nplano;
            _clientes = clientes;
            _instatipo = instatipo;
            _insta = insta;
        }
        public async Task<IActionResult> Index()
        {
            var customers = await _clientes.GetAll();
            var projects = await _proyectos.GetAll();

            ViewBag.CustomersList = customers;
            return View(projects);
        }
        public async Task<IActionResult> Plans()
        {
            var proyectos = await _proyectos.GetAll();
            var nplano = await _nplano.GetAll();

            ViewBag.proyectlist = proyectos;
            return View(nplano);
        }
        public async Task<IActionResult> Instal(int id)
        {
            var instalacion = await _insta.GetAll();
            var proyecto = await _proyectos.GetAll();
            var instalacion_tipo = _instatipo.GetAll();

            SOLDOZA_MST_GRL_PROYECTOS proj = new SOLDOZA_MST_GRL_PROYECTOS();
            proj = proyecto.Where(p => p.id == id).FirstOrDefault();

            List<SOLDOZA_MST_GRL_INSTALACION> instalaciones = new List<SOLDOZA_MST_GRL_INSTALACION>();
            instalaciones = instalacion.Where(i => i.proyecto_id == id).ToList();

            ViewBag.proyecto = proj;
            ViewBag.instalacion_tipo = instalacion_tipo;
            return View(instalaciones);
        }

        [HttpPost]
        public IActionResult AddNPLAN([FromBody] SOLDOZA_MST_PLANOS nplan)
        {
            if (nplan == null)
            {
                return View("NPLAN");
            }

            bool result = _nplano.Insert(nplan);
            if (result)
            {
                return Json(new { exito = true, mensaje = "Successfully added plan" });
            }
            else
            {
                return Json(new { exito = false, mensaje = "Error" });
            }
        }

        [HttpPost]
        public IActionResult EditNPLAN([FromBody] SOLDOZA_MST_PLANOS nplan)
        {
            if (nplan == null)
            {
                return View("NPLAN");
            }

            bool result = _nplano.Update(nplan);
            if (result)
            {
                return Json(new { exito = true, mensaje = "Successfully edit plan" });
            }
            else
            {
                return Json(new { exito = false, mensaje = "Error" });
            }
        }

        [HttpPost]
        public async Task<string> ProjectList(int id)
        {
            string res = "";
            List<SOLDOZA_MST_GRL_PROYECTOS> ListProjects = new List<SOLDOZA_MST_GRL_PROYECTOS>();

            if (id > 0)
            {
                var projects = await _proyectos.GetProjects(id);
                ListProjects = projects.ToList();
            }
            else
            {
                var projects = await _proyectos.GetAll();
                ListProjects = projects.ToList();
            }
            
            if (ListProjects == null)
            {
                return "";
            }

            foreach (var proj in ListProjects)
            {
                res += "<tr>";
                res += "<td>" + proj.cod_proyecto + "</td>";
                res += "<td>" + proj.descripcion_proyecto + "</td>";
                res += "<td>";
                    if (proj.planos.Count > 0)
                    {
                        res += "<table class='table table-sm no-border'>";
                        foreach (var plan in proj.planos)
                        {
                            res += "<tr>";
                            res += "<td>" + plan.cod_num_plano + "</td>";
                            res += "</tr>";                        
                        }
                        res += "</table>";
                    }
                res += "</td>";
                res += "<td>";
                if (proj.instalaciones.Count > 0)
                {
                    res += "<table class='table table-sm no-border'>";
                    foreach (var plan in proj.instalaciones)
                    {
                        res += "<tr>";
                        res += "<td>" + plan.cod_instalacion + "</td>";
                        res += "</tr>";
                    }
                    res += "</table>";
                }
                res += "</td>";
                res += "<td>";
                res += "<a class='btn btn-outline-warning btn-sm' href='@Url.Action('Instal','Projects', new{id=@proj.id})' role='button'>Installations</a>";
                res += "<a class='btn btn-outline-warning btn-sm' href='#' role='button'>Plans</a>";
                res += "<a class='btn btn-outline-warning btn-sm' href='#' role='button'>Revisions</a>";
                res += "<a class='btn btn-outline-warning btn-sm' href='#' role='button'>Pos</a>";
                res += "</td>";
                res += "</tr>";
            }
            return res;
        }
    }
}
