using System;
using System.Collections.Generic;
using System.Net;
using AutoMapper;
using Krosbook.Filters;
using Krosbook.Models.Rooms;
using Krosbook.ViewModels.Rooms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace Krosbook.Controllers.Api.v1
{
    [Route("api/equipment")]
    [EnableCors("AllowAll")]
    [Authorize(Roles = "Admin")]
    public class EquipmentController : BaseController
    {
        #region Private Field

        private IEquipmentRepository _equipmentRepository;
        private ILogger<EquipmentController> _logger;
        private IMapper _mapper;

        #endregion

        #region Constructor
        public EquipmentController(IEquipmentRepository equipmentRepository,
                           ILogger<EquipmentController> logger,
                                                 IMapper mapper)
        {
            _equipmentRepository = equipmentRepository;
            _logger = logger;
            _mapper = mapper;
        }

        #endregion

        #region API

        [HttpGet]
        public IEnumerable<EquipmentViewModel> GetAllEquipmets()
        {
            return _mapper.Map<IEnumerable<EquipmentViewModel>>(_equipmentRepository.GetAll());
        }


        [HttpGet("{equipmentId}", Name = "GetEquipment")]
        public IActionResult GetEquipmentById(int equipmentId)
        {
            var equipment = _equipmentRepository.GetItem(equipmentId);
            if (equipment == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.Json(null);
            }
            else
            {
                return this.Json(_mapper.Map<EquipmentViewModel>(equipment));
            }
        }

        [HttpPost()]
        [ValidateModelState, CheckArgumentsForNull]
        public IActionResult CreateNewEquipment([FromBody] EquipmentViewModel equipmentVm)
        {
            if (!ExistsEquipment(equipmentVm.Description))
            {
                var equipment = _mapper.Map<Equipment>(equipmentVm);

                return SaveData(() =>
                {
                    _equipmentRepository.Add(equipment);
                },
                () =>
                {
                    this.Response.StatusCode = (int)HttpStatusCode.Created;
                    return this.Json(_mapper.Map<EquipmentViewModel>(equipment));
                });
            }
            else
            {
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return this.Json(new { Message = $"Equipment with description '{equipmentVm.Description}' already exists." });
            }
        }

 

        [HttpPut("{equipmentId}")]
        [ValidateModelState, CheckArgumentsForNull]
        public IActionResult UpdateEquipment(int equipmentId, [FromBody] EquipmentViewModel equipmentVm)
        {
            if (equipmentVm.Id != equipmentId)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var message = $"Invalid argument. Id '{equipmentId}' and equipmentVm.Id '{equipmentVm.Id}' are not equal.";
                _logger.LogWarning(message);
                return this.Json(new { Message = message });
            }

            var editedEquipment = _equipmentRepository.GetItem(equipmentId);
            if (editedEquipment == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.Json(null);
            }

            if (ExistsAnotherEquipmentWithName(equipmentVm.Description, equipmentId))
            {
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return this.Json(new { Message = $"Equipment with name '{equipmentVm.Description}' already exists." });
            }
            else
            {
                editedEquipment = _mapper.Map(equipmentVm, editedEquipment);
                return SaveData(() =>
                {
                    _equipmentRepository.Edit(editedEquipment);
                });
            }
        }


        [HttpDelete("{equipmentId}")]
        public IActionResult DeleteEquipmentById(int equipmentId)
        {
            return SaveData(() =>
            {
                _equipmentRepository.Delete(equipmentId);
            });
        }


        #endregion


        #region Helpers
        private bool ExistsEquipment(string description)
        {
            return _equipmentRepository.GetItem(p => p.Description.Equals(description, StringComparison.CurrentCultureIgnoreCase)) != null;
        }


        private IActionResult SaveData(Action beforeAction)
        {
            return SaveData(beforeAction, () => this.Json(null));
        }

        private IActionResult SaveData(Action beforeAction,
                          Func<IActionResult> result)
        {
            try
            {
                beforeAction();
                _equipmentRepository.Save();

                return result();
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured when saving data in EquipmentController.", ex);
                this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return this.Json(new { Message = $"Saving equipment throw Exception '{ex.Message}'" });
            }
        }

        private bool ExistsAnotherEquipmentWithName(string equipmentDescription, int equipmentId)
        {
            var equipment = _equipmentRepository.GetItem(p => p.Description.Equals(equipmentDescription, StringComparison.CurrentCultureIgnoreCase));

            return equipment != null && equipment.Id != equipmentId;
        }

        #endregion
    }
}