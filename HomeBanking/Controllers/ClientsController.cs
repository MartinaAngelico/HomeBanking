﻿using HomeBanking.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using HomeBanking.Repositories;
using System.Linq;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    //hereda los controles
    {
        private IClientRepository _clientRepository; //se declara de forma privada la INTERFAZ
        private IAccountRepository _accountRepository;
        public ClientsController(IClientRepository clientRepository, IAccountRepository accountRepository) //constructor  que usa dicha INTERFAZ 
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
        }
    [HttpGet] //cuando hagamos un peticion de tipo get al controlador va a responder con el sgte metodo
        public IActionResult Get()
        {
            try
            {
                var clients = _clientRepository.GetAllClients(); //el GEtallClients incluye las cuentas
                //con var no especificamos el tipo de dato
                var clientsDTO = new List<ClientDTO>(); //variable DTO xq no queremos mostrar todos los datos

                foreach (Client client in clients) //recorremos
                {
                    var newClientDTO = new ClientDTO //creamos nuevos cliente DTO
                    {
                        Id = client.Id,
                        Email = client.Email,
                        FirstName = client.FirstName,
                        LastName = client.LastName,
                        Accounts = client.Accounts.Select(ac => new AccountDTO //SELECT metodo de linq para modificar datos
                        {
                            Id = ac.Id,
                            Balance = ac.Balance,
                            CreationDate = ac.CreationDate,
                            Number = ac.Number
                        }).ToList(),
                    };
                    clientsDTO.Add(newClientDTO);
                }
                return Ok(clientsDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                var client = _clientRepository.FindById(id);
                if (client == null)
                {
                    return NotFound(); //no encontro el cliente
                }
                var clientDTO = new ClientDTO
                {
                    Id = client.Id,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Accounts = client.Accounts.Select(ac => new AccountDTO
                    { 
                        Id = ac.Id,
                        Balance = ac.Balance,
                        CreationDate = ac.CreationDate,
                        Number = ac.Number
                    }).ToList(),

                    Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                    {
                        Id = cl.Id,
                        LoanId = cl.LoanId,
                        Name = cl.Loan.Name,
                        Amount = cl.Amount,
                        Payments = int.Parse(cl.Payments)
                    }).ToList(), //devuelve los prestamos asociados al cliente

                    Cards = client.Cards.Select(c => new CardDTO
                    {
                        Id = c.Id,
                        CardHolder = c.CardHolder,
                        Color = c.Color,
                        Cvv = c.Cvv,
                        FromDate = c.FromDate,
                        Number = c.Number,
                        ThruDate = c.ThruDate,
                        Type = c.Type
                    }).ToList(),
                };
                return Ok(clientDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("current")] //nos trae nuestra info en base a los datos que proporcionemos (mail y contraseña)
        public IActionResult GetCurrent()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty; //traemos el mail de la base de datos 
                              //este USER proviene del sistema de autenticacion que tenemos para manejar en Back, llamamos un objeto especial que tiene la info de un cliente como usuario de nuestro Back 
                              
                if (email == string.Empty)
                {
                    return Forbid();
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null) //lo buscamos como cliente en nuestra base de datos
                {
                    return Forbid();
                }

                var clientDTO = new ClientDTO //si lo encontramos en la base de datos, traemos todos los datos
                {
                    Id = client.Id,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Accounts = client.Accounts.Select(ac => new AccountDTO
                    {
                        Id = ac.Id,
                        Balance = ac.Balance,
                        CreationDate = ac.CreationDate,
                        Number = ac.Number
                    }).ToList(),
                    Credits = client.ClientLoans.Select(cl => new ClientLoanDTO //a los loans le llamamos credits
                    {
                        Id = cl.Id,
                        LoanId = cl.LoanId,
                        Name = cl.Loan.Name,
                        Amount = cl.Amount,
                        Payments = int.Parse(cl.Payments)
                    }).ToList(),
                    Cards = client.Cards.Select(c => new CardDTO
                    {
                        Id = c.Id,
                        CardHolder = c.CardHolder,
                        Color = c.Color,
                        Cvv = c.Cvv,
                        FromDate = c.FromDate,
                        Number = c.Number,
                        ThruDate = c.ThruDate,
                        Type = c.Type
                    }).ToList()
                };

                return Ok(clientDTO); //muestra el cliente en el Front
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost] //CREAR UN NUEVO CLIENTE
        //un POST y no un GET porque enviamos datos del Front al Back y el GET es para solicitar datos del Back
        public IActionResult Post([FromBody] Client client)
        {
            try
            {
                //validamos datos antes
                if (String.IsNullOrEmpty(client.Email) || String.IsNullOrEmpty(client.Password) || String.IsNullOrEmpty(client.FirstName) || String.IsNullOrEmpty(client.LastName))
                    return StatusCode(403, "datos inválidos"); //el 403 es = que el Forbid 

                //buscamos si ya existe el usuario
                Client user = _clientRepository.FindByEmail(client.Email); //buscamos en el repositorio
                if (user != null)
                {
                    return StatusCode(403, "Email está en uso");
                }
                
                Client newClient = new Client //creamos nuevo objeto de tipo cliente
                {
                    Email = client.Email,
                    Password = client.Password,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                };
                _clientRepository.Save(newClient); //usamos el repo para utilizar el metodo SAVE 

                Random random = new Random();
                string numeroAleatorio = random.Next(0, 100000000).ToString("D8");
                Account newAccount = new Account
                {
                    Number = "VIN-" + numeroAleatorio,
                    CreationDate = DateTime.Now,
                    Balance = 0,
                    ClientId = newClient.Id,
                };
                _accountRepository.Save(newAccount);

                ClientDTO newCDTO = new ClientDTO
                {
                    Id = newClient.Id,
                    Email = newClient.Email,
                    FirstName = newClient.FirstName,
                    LastName = newClient.LastName,
                    Accounts = newClient.Accounts.Select(account => new AccountDTO
                    {
                        Id = account.Id,
                        Number = account.Number,
                        CreationDate = account.CreationDate,
                        Balance = account.Balance,
                    }).ToList()
                };
                return Created("", newCDTO); //le devolvemos el cliente 

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
