using HomeBanking.Models;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Net;
using System.Security.Principal;

namespace HomeBanking.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AccountsController : ControllerBase
    //hereda los controles
    {
        private IAccountRepository _accountRepository;
        private IClientRepository _clientRepository;
        //necesitamos un repositorio 

        public AccountsController(IAccountRepository accountRepository, IClientRepository clientRepository) //constructor
        {
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;

        }

        [HttpGet] //cuando hagamos un peticion de tipo get al controlador va a responder con el sgte metodo
        public IActionResult Get()
        {
            try
            {
                var accounts = _accountRepository.GetAllAccounts(); //el GEtallClients incluye las cuentas
                //con var no especificamos el tipo de dato
                var AccountDTO = new List<AccountDTO>(); //variable DTO xq no queremos mostrar todos los datos

                foreach (Account account in accounts) //recorremoss
                {
                    var newAccountDTO = new AccountDTO //creamos nuevos cliente DTO
                    {
                        Id = account.Id,
                        Number = account.Number,
                        CreationDate = account.CreationDate,
                        Balance = account.Balance,
                        Transactions = account.Transactions.Select(tr => new TransactionDTO //SELECT metodo de linq para modificar datos
                        {
                            Id = tr.Id,
                            Type = tr.Type,
                            Amount = tr.Amount,
                            Description = tr.Description,
                            Date = tr.Date,
                        }).ToList()
                    };
                    AccountDTO.Add(newAccountDTO);
                }
                return Ok(AccountDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("accounts/{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                var account = _accountRepository.FindById(id);
                if (account == null)
                {
                    return NotFound(); //no encontro el cliente
                }
                var accountDTO = new AccountDTO
                {
                    Id = account.Id,
                    Number = account.Number,
                    CreationDate = account.CreationDate,
                    Balance = account.Balance,
                    Transactions = account.Transactions.Select(tr => new TransactionDTO //SELECT metodo de linq para modificar datos
                    {
                        Id = tr.Id,
                        Type = tr.Type,
                        Amount = tr.Amount,
                        Description = tr.Description,
                        Date = tr.Date,
                    }).ToList()
                };
                return Ok(accountDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //TAREA 7
        [HttpPost("clients/current/accounts")]
        public IActionResult Post()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return StatusCode(403, "Unauthorized client");
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return StatusCode(403, "Client not found");
                }

                if (client.Accounts.Count() > 2)
                {
                    return StatusCode(403, "Account limit per client reached");
                }
                Random random = new Random();
                string numeroAleatorio = random.Next(0, 100000000).ToString("D8");
                Account newAccount = new Account
                {
                    Number = "VIN-" + numeroAleatorio,
                    CreationDate = DateTime.Now,
                    Balance = 0,
                    ClientId = client.Id,
                };
                _accountRepository.Save(newAccount);
                AccountDTO newAccountDTO = new AccountDTO
                {
                    Number = newAccount.Number,
                    CreationDate = newAccount.CreationDate,
                    Balance = 0,
                };
                return Created("", newAccountDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("clients/current/accounts")]
        public IActionResult GetCurrent()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return StatusCode(403, "Unauthorized client");
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return StatusCode(403, "Client not found");
                }
                var accountsDTO = new List<AccountDTO>();
                foreach (Account account in client.Accounts) //recorremoss
                {
                    var newAccountDTO = new AccountDTO //creamos nuevos cliente DTO
                    {
                        Id = account.Id,
                        Number = account.Number,
                        CreationDate = account.CreationDate,
                        Balance = account.Balance,
                        Transactions = account.Transactions.Select(tr => new TransactionDTO //SELECT metodo de linq para modificar datos
                        {
                            Id = tr.Id,
                            Type = tr.Type,
                            Amount = tr.Amount,
                            Description = tr.Description,
                            Date = tr.Date,
                        }).ToList()
                    };
                    accountsDTO.Add(newAccountDTO);  
                }
                return Ok(accountsDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }
    }
}
