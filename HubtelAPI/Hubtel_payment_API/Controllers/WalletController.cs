using Hubtel_payment_API.Repositories.Wallets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;

namespace Hubtel_payment_API.Controllers

{
    [Route("/api")]
    [ApiController]
    public class WalletController: ControllerBase
    {
        private readonly IWalletRepository _walletRepository;

        public WalletController(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        [HttpGet("GetAllWallets")]
        public async Task<IActionResult> GetAllWallets()
        {
            var obj = await _walletRepository.GetAllWalletsAsync();
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

        [HttpGet("GetWalletById/{id}", Name ="GetWalletById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task <IActionResult> GetWalletById(Guid id)
        {
            if (id == Guid.Empty || id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                return BadRequest();
            }

            var obj = await _walletRepository.GetSingleWalletAsync(id);

            if (obj == null)
            {
                return NotFound();  
            }

            return Ok(obj);
        }

        [HttpDelete("DeleteById/{id}", Name ="DeleteById")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task <IActionResult> DeleteById(Guid id)
        {
            if (id == Guid.Empty || id == Guid.Parse("00000000-0000-0000-0000-000000000000")) 
            {
                return BadRequest();
            }
     
            await _walletRepository.DeleteWalletAsync(id);

            return NoContent();
        }

        [HttpPost("CreateWallet")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task <IActionResult> CreateWallet([FromBody] Entities.Wallets details)
        {
            if (details == null)
            {
                return BadRequest();
            }

            details.WalletAccountNumber = details.AccountNumber;

            //To prevent duplicate wallet number in the system so it will return 409 conflict status
            //code which the account number is already in the system 
            if (await _walletRepository.CheckIfWalletAccountExistsAsync(details.AccountNumber))
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }
            

            

            //To prevent a single User to have more than 5 wallets
            var vUserCount = await _walletRepository.GetWalletCountAsync(details.WalletAccountNumber);
            int iUserCount = vUserCount.Count();
            if (iUserCount >= 5)
            {
                throw new Exception("User cannot have more than 5 wallets");
            }



            //To prevent the user to save any type of wallet other than Card and Momo
            if (!(details.Type.ToLower().Equals("card") || details.Type.ToLower().Equals("momo")))
            {
                throw new Exception($"Wallet type {details.Type} does not exist");
            }
            


            //To prevent the user to save any account scheme other than Visa, MasterCard, MTN, AirtelTigo or Vodafone
            if (!(details.AccountScheme.ToUpper().Equals("VISA") || details.AccountScheme.ToUpper().Equals("MASTERCARD")
                || details.AccountScheme.ToUpper().Equals("MTN") || details.AccountScheme.ToUpper().Equals("VODAFONE")
                || details.AccountScheme.ToUpper().Equals("AIRTELTIGO")))
            {
                throw new Exception($"Account scheme {details.AccountScheme} does not exist");
            }

            

            if(details.AccountNumber != null )
            {
                if(details.Type.ToLower().Equals("momo"))
                {
                    if (details.WalletAccountNumber.Length > 10 || details.WalletAccountNumber.Length < 10)
                    {
                        throw new Exception($"Momo Account Number {details.WalletAccountNumber} can not be more or less than 10 characters");


                    }

                }
                else 
                {

                    if ( details.Type.ToUpper().Equals("CARD"))

                    {
                        if (details.WalletAccountNumber.Length > 16 || details.WalletAccountNumber.Length < 16)
                        {
                            throw new Exception($"Card  Number {details.WalletAccountNumber} can not be more or less than 16 characters");



                        }

                    }
                }



            }
            else
            {
                throw new Exception("Account number cannot be null");

            }





            if (details.AccountNumber != null && details.Type.ToLower().Equals("momo") && details.AccountScheme.ToLower().Equals("mtn") )
            {
                if(!(details.AccountNumber.Contains("055") || details.AccountNumber.Contains("024") || details.AccountNumber.Contains("059")))
                {
                    throw new Exception($"Account number is not a valid MTN Number");

                }

            }
            if (details.AccountNumber != null && details.Type.ToLower().Equals("momo") )
            {
                if (!(details.AccountScheme.ToLower().Equals("airteltigo") || details.AccountScheme.ToLower().Equals("mtn") || details.AccountScheme.ToLower().Equals("vodafone")))
                {
                    throw new Exception("Account scheme does not correspond with account type ");

                }

            }
            if (details.AccountNumber != null && details.Type.ToLower().Equals("card"))
            {
                if (!(details.AccountScheme.ToLower().Equals("visa") || details.AccountScheme.ToLower().Equals("mastercard")))
                {
                    throw new Exception("Account scheme does not correspond with account type ");

                }

            }
            if (details.AccountNumber != null && details.Type.ToLower().Equals("momo") && details.AccountScheme.ToLower().Equals("airteltigo")  )
            {
                if((details.AccountNumber.Contains("027") || details.AccountNumber.Contains("056") || details.AccountNumber.Contains("026") || details.AccountNumber.Contains("057")))
                {
                    throw new Exception($"Account number is not a valid AirtelTigo Number");

                }


            }
            if (details.AccountNumber != null && details.Type.ToLower().Equals("momo") && details.AccountScheme.ToLower().Equals("vodafone") )
            {
                if((details.AccountNumber.Contains("020") || !details.AccountNumber.Contains("050")))
                {
                    throw new Exception($"Account number is not a valid Vodafone Number");

                }

            }
            if (details.AccountNumber != null && details.Type.ToLower().Equals("card") && details.AccountScheme.ToLower().Equals("visa") && !details.AccountNumber.StartsWith("4"))
            {

                throw new Exception($"Account number is not a valid Visa Card Number");

            }
            //To only check if the master card starts 5 and is a valid number
            if (details.AccountNumber != null && details.Type.ToLower().Equals("card") && details.AccountScheme.ToLower().Equals("mastercard") && !details.AccountNumber.StartsWith("5"))
            {

                throw new Exception($"Account number is not a valid MasterCard Number");

            }

            if (details.AccountNumber != null )
            {
                long numericValue;
                bool isNum = Int64.TryParse(details.AccountNumber, out numericValue);
                if (isNum == false)
                {
                    throw new Exception($"Account number is not a valid  Number");

                }




            }

            //To only get first 6 digits of a Master Card or Visa Card number


            string strValue = string.Empty;
            if (details.AccountScheme.ToUpper().Equals("VISA") || details.AccountScheme.ToUpper().Equals("MASTERCARD"))
            {
                strValue = details.AccountNumber.ToString().Substring(0, 6);
                details.AccountNumber = strValue;
            }
            else
            {
                details.AccountNumber = details.AccountNumber;
            }

            details.WalletId = Guid.NewGuid();

            //Save to the Wallet table
            await _walletRepository.AddWalletAsync(details);

            return Ok(details);
        }
    }
}
