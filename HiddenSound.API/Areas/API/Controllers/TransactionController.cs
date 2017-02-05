using System;
using System.Linq;
using System.Security.Cryptography;
using HiddenSound.API.Areas.API.Models.Responses;
using HiddenSound.API.Areas.API.Services;
using HiddenSound.API.Areas.Shared.Models;
using HiddenSound.API.Areas.Shared.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HiddenSound.API.Areas.API.Controllers
{
    [Area("API")]
    [Route("Api/[controller]")]
    [EnableCors("API")]
    public class TransactionController : Controller
    {
        public IQRService QRService { get; set; }

        public ITransactionRepository TransactionRepository { get; set; }

        [HttpGet]
        [Route("[action]")]
        public ActionResult List()
        {
            return Json(TransactionRepository.GetAllTransactions());
        }

        [HttpPost]
        [Route("[action]")]
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

            var authorization = new Transaction
            {
                AuthorizationCode = authorizationCode,
                Base64QR = qrCode,
                ExpiresOn = DateTime.UtcNow.AddSeconds(5 * 60),
                Status = TransactionStatus.Pending,
                UserID = 0,
                VendorID = 0
            };

            TransactionRepository.CreateTransaction(authorization);

            var response = new TransactionCreateResponse
            {
                AuthorizationCode = authorizationCode,
                Base64QR = qrCode
            };
            return Json(response);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(TransactionStatusResponse), 200)]
        [ProducesResponseType(typeof(TransactionStatusResponse), 404)]
        public ActionResult Status([FromQuery] string authorizationCode)
        {
            var authorization = TransactionRepository.GetTransaction(authorizationCode);

            if (authorization == null)
            {
                return NotFound();
            }

            // we won't do any updating here. expired date is already stored
            // and we can use batch sql update job to remove expired authorizations
            if (DateTime.UtcNow >= authorization.ExpiresOn)
            {
                authorization.Status = TransactionStatus.Expired;
            }

            var response = new TransactionStatusResponse
            {
                Status = authorization.Status
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(Transaction), 200)]
        [ProducesResponseType(typeof(TransactionStatusResponse), 404)]
        public ActionResult Info([FromQuery] string authorizationCode)
        {
            var authorization = TransactionRepository.GetTransaction(authorizationCode);

            if (authorization == null)
            {
                return NotFound();
            }

            return Json(authorization);
        }
    }
}
