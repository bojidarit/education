﻿namespace NetCoreApi.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using NetCoreApi.Dtos;
	using System;
	using System.Collections.Generic;
	using System.Linq;

	[Route("api/[controller]")]
	[ApiController]
	public class DemoController : ControllerBase
	{
		private static List<IdNameDto> dummyList = Enumerable.Range(1, 10)
			.Select(i => IdNameDto.Create(i, $"Name {i}"))
			.ToList();

		[HttpGet]
		public ActionResult<IdNameDto> GetAll()
		{
			return Ok(dummyList);
		}

		[HttpGet("{id}")]
		public ActionResult<IdNameDto> GetById(int id)
		{
			var dto = dummyList.Where(i => i.Id == id).FirstOrDefault();
			if (dto == null)
			{
				return NotFound(ErrorDto.Create($"Item with id = {id} not found."));
			}

			return Ok(dto);
		}

		[HttpPost]
		public IActionResult Create(IdNameDto dto)
		{
			if (dto == null)
			{
				return NoContent();
			}

			if (dummyList.Any(i => i.Id == dto.Id))
			{
				return BadRequest(ErrorDto.Create($"Item with id = {dto.Id} already exists."));
			}

			dummyList.Add(dto);

			Uri uri = Helper.CombineRequestPath(this.Request, dto.Id.ToString());
			return Created(uri, dto);
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			var dto = dummyList.Where(i => i.Id == id).FirstOrDefault();
			if (dto == null)
			{
				return NotFound(ErrorDto.Create($"Item with id = {id} not found."));
			}

			dummyList.Remove(dto);

			return NoContent();
		}

		[HttpPut]
		public IActionResult Update(IdNameDto dto)
		{
			if (dto == null)
			{
				return NoContent();
			}

			var found = dummyList.Where(i => i.Id == dto.Id).FirstOrDefault();
			if (found == null)
			{
				return BadRequest(ErrorDto.Create($"Item with id = {dto.Id} do not exists."));
			}

			dummyList.Remove(found);

			dummyList.Add(dto);

			Uri uri = Helper.CombineRequestPath(this.Request, dto.Id.ToString());
			return Created(uri, dto);
		}
	}
}
