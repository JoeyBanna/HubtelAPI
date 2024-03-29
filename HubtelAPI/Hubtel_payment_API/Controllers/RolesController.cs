﻿using Confluent.Kafka;
using Hubtel_payment_API.Repositories.Roles;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Hubtel_payment_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RolesController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var obj = await _roleRepository.GetAllRolesAsync();

                if (obj is null)
                {
                    return NotFound();
                }

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }

        [HttpPost("PostRoles")]
        public async Task<IActionResult> PostRoles(Entities.Roles model)
        {
            if (!ModelState.IsValid) { return BadRequest(); }

            if (model is null) { return BadRequest(); }

            model.RoleId = Guid.NewGuid();

            await _roleRepository.AddRoleAsync(model);


            // KAKFA
            string serializedWallet = JsonConvert.SerializeObject(model);

            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            using var producera = new ProducerBuilder<Null, string>(config).Build();

            var response = await producera.ProduceAsync("ProducerMessage", new Message<Null, string> { Value = serializedWallet });
            Console.WriteLine($"deliverd: {response.Value} to {response.TopicPartitionOffset}");

            return Ok(model);
        }
    }
}
