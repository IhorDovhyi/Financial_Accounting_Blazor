using Financial_Accounting_Blazor.Models;
using Financial_Accounting_Blazor.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Financial_Accounting_Blazor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoneyTransactionController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public MoneyTransactionController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [Route("~/api/GetAll")]
        [HttpGet]
        public async Task<ActionResult<List<Transaction>>> GetAll()
        {
            UseCaseResult all = JsonSerializer.Deserialize<UseCaseResult>(new GetOperationsUseCase(this.unitOfWork).Execute());

            if (all == null)
            {
                return NotFound();
            }
            return Ok(all);
        }

        [Route("~/api/GetAllIncome")]
        [HttpGet]
        public async Task<ActionResult<List<Transaction>>> GetIncome()
        {
            UseCaseResult income = JsonSerializer.Deserialize<UseCaseResult>(new IncomeUseCase(this.unitOfWork).Execute());
            if (income.status)
            {
                return income.result;
            }
            else
            {
                return NotFound();
            }
        }

        [Route("~/api/GetAllConsumption")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetConsumption()
        {
            UseCaseResult consumption = JsonSerializer.Deserialize<UseCaseResult>(new ConsumptionUseCase(this.unitOfWork).Execute());
            if (consumption.status)
            {
                return consumption.result;
            }
            else
            {
                return NotFound();
            }
        }

        [Route("~/api/PostTransaction")]
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            if (IsValid(transaction))
            {
                if (JsonSerializer.Deserialize<UseCaseResult>(new PostUseCase(this.unitOfWork).Execute(transaction)).status)
                {
                    return Ok(transaction);
                }
                else
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [Route("~/api/PostTransactions")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Transaction>>> PostTransactions(List<Transaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                if (IsValid(transaction))
                {
                    return BadRequest();
                }
            }
            if (JsonSerializer.Deserialize<UseCaseResult>(new PostUseCase(this.unitOfWork).Execute(transactions)).status)
            {
                return Ok(transactions);
            }
            else
            {
                return NotFound();
            }
        }

        [Route("~/api/PutTransaction")]
        [HttpPut]
        public async Task<ActionResult<Transaction>> PutTransaction(Transaction transaction)
        {
            if (IsValid(transaction))
            {
                return BadRequest();
            }
            if (JsonSerializer.Deserialize<UseCaseResult>(new PutUseCase(this.unitOfWork).Execute(transaction)).status)
            {
                return Ok(transaction);
            }
            else
            {
                return NotFound();
            }
        }

        [Route("~/api/PutTransactions")]
        [HttpPut]
        public async Task<ActionResult<IEnumerable<Transaction>>> PutTransactions(List<Transaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                if (IsValid(transaction))
                {
                    return BadRequest();
                }
            }
            if (JsonSerializer.Deserialize<UseCaseResult>(new PutUseCase(this.unitOfWork).Execute(transactions)).status)
            {
                return Ok(transactions);
            }
            else
            {
                return NotFound();
            }

        }

        [Route("~/api/DeleteTransaction/{Id}")]
        [HttpDelete]
        public async Task<ActionResult<Transaction>> DeleteTransaction(int Id)
        {
            if (IsValid(Id))
            {
                if (JsonSerializer.Deserialize<UseCaseResult>(new DeleteUseCase(this.unitOfWork).Execute(Id)).status)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            return BadRequest();
        }

        [Route("~/api/DeleteTransactions/{Ids}")]
        [HttpDelete]
        public async Task<ActionResult<IEnumerable<Transaction>>> DeleteTransactions(int[] Ids)
        {
            foreach (var id in Ids)
            {
                if(!IsValid(id))
                {
                    return BadRequest();
                }
            }

            if (JsonSerializer.Deserialize<UseCaseResult>(new DeleteUseCase(this.unitOfWork).Execute(Ids)).status)
            {
                return Ok(Ids);
            }
            else
            {
                return NotFound();
            }
        }

        [Route("~/api/GetIncomePerDate/{date}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetIncomePerDate(DateTime date)
        {
            if (IsValid(date))
            {
                UseCaseResult IncomePerDate = JsonSerializer.Deserialize<UseCaseResult>(new IncomeUseCase(this.unitOfWork).Execute(date));

                if (IncomePerDate.status)
                {
                    return IncomePerDate.result;
                }
                else if (IncomePerDate.messageCode == 404)
                {
                    return NotFound();
                }
            }
            return BadRequest();
        }

        [Route("~/api/GetOperationsPerDate/{date}")]
        [HttpGet]
        public async Task<int> GetOperationsPerDate(DateTime date)
        {
            if (IsValid(date))
            {
                return JsonSerializer.Deserialize<UseCaseResult>(new GetOperationsUseCase(this.unitOfWork).Execute(date)).result.Count;
            }
            else
            {
                return 0;
            }
        }

        [Route("~/api/GetConsumptionPerDate/{date}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetConsumptionPerDate(DateTime date)
        {
            if (IsValid(date))
            {
                UseCaseResult ConsumptionPerDate = JsonSerializer.Deserialize<UseCaseResult>(new ConsumptionUseCase(this.unitOfWork).Execute(date));

                if (ConsumptionPerDate.status)
                {
                    return ConsumptionPerDate.result;
                }
                else if (ConsumptionPerDate.messageCode == 404)
                {
                    return NotFound();
                }
            }
            return BadRequest();
        }

        [Route("~/api/GetIncomeForThePeriod/{start, end}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetIncomeForThePeriod(DateTime start, DateTime end)
        {
            if (IsValid(start) && IsValid(end))
            {
                UseCaseResult IncomeForThePeriod = JsonSerializer.Deserialize<UseCaseResult>(new IncomeUseCase(this.unitOfWork).Execute(start, end));
                if (IncomeForThePeriod.status)
                {
                    return IncomeForThePeriod.result;
                }
                else if (IncomeForThePeriod.messageCode == 404)
                {
                    return NotFound();
                }
            }
            return BadRequest();
        }

        [Route("~/api/GetConsumptionForThePeriod/{start, end}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetConsumptionForThePeriod(DateTime start, DateTime end)
        {
            if (IsValid(start) && IsValid(end))
            {
                UseCaseResult ConsumptionForThePeriod = JsonSerializer.Deserialize<UseCaseResult>(new ConsumptionUseCase(this.unitOfWork).Execute(start, end));
                if (ConsumptionForThePeriod.status)
                {
                    return ConsumptionForThePeriod.result;
                }
                else if (ConsumptionForThePeriod.messageCode == 404)
                {
                    return NotFound();
                }
            }
                return BadRequest();
        }

        [Route("~/api/GetOperationsForThePeriod/{start, end}")]
        [HttpGet]
        public async Task<ActionResult<int>> GetOperationsForThePeriode(DateTime start, DateTime end)
        {
            if (IsValid(start) && IsValid(end))
            {
                UseCaseResult OperationsForThePeriode = JsonSerializer.Deserialize<UseCaseResult>(new GetOperationsUseCase(this.unitOfWork).Execute(start, end));
                if (OperationsForThePeriode.status)
                {
                    int operationsForThePeriode = OperationsForThePeriode.result.Count();
                    return operationsForThePeriode;
                }
                else if (OperationsForThePeriode.messageCode == 404)
                {
                    return NotFound();
                }
            }
            return 0;
        }

        protected bool IsValid(Transaction transaction)
        {
            return transaction != null;
        }

        protected bool IsValid(int id)
        {
            return id != 0 && id > 0;
        }

        protected bool IsValid(DateTime validDate)
        {
            return validDate != null;
        }
    }
}
