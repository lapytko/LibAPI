// using System;
// using System.ComponentModel.DataAnnotations;
// using System.Threading.Tasks;
// using fair_mark_api.Extensions;
// using fair_mark_api.Facade;
// using fair_mark_api.Models.Entity;
// using fair_mark_api.Models.FairApi;
// using fair_mark_api.Models.Posts;
// using fair_mark_api.Models.ResponseResult;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
//
// namespace fair_mark_api.Controllers
// {
//     [Authorize]
//     [Route("settings")]
//     [ApiController]
//     public class SettingsController : BaseController
//     {
//         private readonly SettingsServiceFacade _settingsServiceFacade;
//
//         public SettingsController(SettingsServiceFacade settingsServiceFacade)
//         {
//             _settingsServiceFacade = settingsServiceFacade;
//         }
//
//         [HttpPost("check-oms")]
//         public async Task<IActionResult> CheckOms([FromBody] string certificateNumber)
//         {
//             try
//             {
//                 return ResponseResult.Success(await _settingsServiceFacade.CheckOms(certificateNumber));
//             }
//             catch (Exception e)
//             {
//                 return ResponseResult.Error(e);
//             }
//         }
//
//         /// <summary>
//         /// Data validation: OMS, clientToken, connectionId | Проверка валидности данных: OMS, clientToken, connectionId
//         /// </summary>
//         /// <returns></returns>
//         [HttpGet("validate-oms")]
//         public async Task<IActionResult> ValidateOms(
//             [Required(ErrorMessage = "Обязательный HEADER (\"fair-token\")")] [FromHeader(Name = FairTokenHeader)]
//             string token)
//         {
//             try
//             {
//                 var (isValidate, connectionId) = await _settingsServiceFacade.ValidateOms(token);
//                 return ResponseResult.Success(new
//                 {
//                     isValidate,
//                     connectionId
//                 });
//             }
//             catch (Exception e)
//             {
//                 return ResponseResult.Error(e);
//             }
//         }
//
//         /// <summary>
//         /// Set ClientToken for cert
//         /// </summary>
//         /// <returns></returns>
//         [HttpPost("set-client-token")]
//         [ProducesResponseType(typeof(ResponseResult<bool>), 200)]
//         public async Task<ResponseResult<bool>> SetClientToken(
//             [Required] [FromHeader(Name = FairTokenHeader)] string fairToken,
//             [Required] [FromHeader(Name = "fair-client-token")]
//             string clientToken)
//         {
//             return (await _settingsServiceFacade.SetClientToken(fairToken, clientToken)).ToResponseResult();
//         }
//
//         /// <summary>
//         /// Save Oms data | Сохранение значений Oms
//         /// </summary>
//         /// <param name="omsModel"></param>
//         /// <returns></returns>
//         [HttpPost("oms")]
//         public async Task<IActionResult> SaveOms(OmsData omsModel)
//         {
//             try
//             {
//                 Request.Headers.TryGetValue("fair-token", out var token);
//                 return ResponseResult.Success(await _settingsServiceFacade.SaveOms(omsModel, token));
//             }
//             catch (Exception e)
//             {
//                 return ResponseResult.Error(e);
//             }
//         }
//
//         [HttpGet("oms")]
//         public async Task<IActionResult> GetOms()
//         {
//             try
//             {
//                 return ResponseResult.Success(await _settingsServiceFacade.GetOms());
//             }
//             catch (Exception e)
//             {
//                 return ResponseResult.Error(e);
//             }
//         }
//
//         /// <summary>
//         ///  Get info about Company | Получение информации об организации
//         /// </summary>
//         /// <param name="token">токен честного знака</param>
//         /// <returns></returns>
//         [HttpGet("about-cert")]
//         [ProducesResponseType(typeof(ResponseResult<FairCompany>), 200)]
//         public async Task<ResponseResult<FairCompany>> GetAboutCert(
//             [Required] [FromHeader(Name = "fair-token")]
//             string token)
//         {
//             return (await _settingsServiceFacade.GetAboutOrganization(token)).ToResponseResult();
//         }
//
//         /// <summary>
//         ///  Save logs | Сохранение логов 
//         /// </summary>
//         /// <param name="logForSave">Лог для сохранения</param>
//         /// <returns></returns>
//         [HttpPost("log")]
//         [ProducesResponseType(typeof(ResponseResult<bool>), 200)]
//         public async Task<ResponseResult<bool>> LogSave([FromBody] FrontLogModel logForSave)
//         {
//             return (await _settingsServiceFacade.LogSave(logForSave)).ToResponseResult();
//         }
//
//         /// <summary>
//         /// Get url for sign connectionId | Получение URL для подписи (connectionId)
//         /// </summary>
//         /// <param name="omsId"></param>
//         /// <returns></returns>
//         [HttpGet("signature-connectionId/{omsId}")]
//         [ProducesResponseType(typeof(ResponseResult<string>), 200)]
//         public ResponseResult<string> GetStrForSign([FromRoute] string omsId)
//         {
//             return _settingsServiceFacade.GetStrForSign(omsId).ToResponseResult();
//         }
//
//         /// <summary>
//         /// Get connectionId | Получение connectionId
//         /// </summary>
//         /// <param name="model"></param>
//         /// <returns></returns>
//         [HttpPost("connectionId")]
//         [ProducesResponseType(typeof(ResponseResult<Guid>), 200)]
//         public async Task<ResponseResult<Guid>> GetStrForSign([FromBody] FrontConnectionSignModel model)
//         {
//             return (await _settingsServiceFacade.GetConnectionId(model)).ToResponseResult();
//         }
//     }
// }