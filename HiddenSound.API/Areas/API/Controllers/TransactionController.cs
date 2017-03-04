using System;
using System.Linq;
using System.Security.Cryptography;
using HiddenSound.API.Areas.API.Models.Responses;
using HiddenSound.API.Areas.API.Services;
using HiddenSound.API.Areas.Shared.Models;
using HiddenSound.API.Areas.Shared.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HiddenSound.API.Areas.API.Controllers
{
    [Area("API")]
    [Route("Api/[controller]")]
    public class TransactionController : Controller
    {
        public IQRService QRService { get; set; }

        public ITransactionRepository TransactionRepository { get; set; }

        [HttpGet("[action]")]
        [Authorize("Api")]
        public ActionResult List()
        {
            return Json(TransactionRepository.GetAllTransactions());
        }

        [HttpPost("[action]")]
        [Authorize("Api")]
        [ProducesResponseType(typeof(TransactionCreateResponse), 200)]
        public ActionResult Create()
        {
            var salt = new byte[256 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var authorizationCode = Convert.ToBase64String(salt);
            var qrCode = QRService.Create(authorizationCode);

            var transaction = new Transaction
            {
                AuthorizationCode = authorizationCode,
                Base64QR = qrCode,
                ExpiresOn = DateTime.UtcNow.AddSeconds(5 * 60),
                Status = TransactionStatus.Pending,
                UserId = Guid.NewGuid(),
                VendorId = Guid.NewGuid()
            };

            TransactionRepository.CreateTransaction(transaction);

            var response = new TransactionCreateResponse
            {
                AuthorizationCode = authorizationCode,
                Base64QR = qrCode
            };
            return Json(response);
        }

        [HttpGet("[action]")]
        [Authorize("Api")]
        [ProducesResponseType(typeof(TransactionStatusResponse), 200)]
        [ProducesResponseType(typeof(TransactionStatusResponse), 404)]
        public ActionResult Status([FromQuery] string authorizationCode)
        {
            var transaction = TransactionRepository.GetTransaction(authorizationCode);

            if (transaction == null)
            {
                return NotFound();
            }

            // we won't do any updating here. expired date is already stored
            // and we can use batch sql update job to remove expired authorizations
            if (DateTime.UtcNow >= transaction.ExpiresOn)
            {
                transaction.Status = TransactionStatus.Expired;
            }

            var response = new TransactionStatusResponse
            {
                Status = transaction.Status
            };
            return Ok(response);
        }

        [HttpGet("[action]")]
        [Authorize("Api")]
        [ProducesResponseType(typeof(Transaction), 200)]
        [ProducesResponseType(typeof(TransactionStatusResponse), 404)]
        public ActionResult Info([FromQuery] string authorizationCode)
        {
            var transaction = TransactionRepository.GetTransaction(authorizationCode);

            if (transaction == null)
            {
                return NotFound();
            }

            transaction.Base64QR = QRService.Create(transaction.AuthorizationCode);

            return Json(transaction);
        }
    }
}
