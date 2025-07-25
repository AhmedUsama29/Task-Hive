﻿using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Services.Specifications;
using ServicesAbstraction;
using Shared.DataTransferObjects.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TaskService(IUnitOfWork _unitOfWork, IMapper _mapper) : ITaskService
    {
        public async Task<TaskDetailedResponse> CreateTaskAsync(TaskCreationDto taskCreationDto)
        {
            var repo = _unitOfWork.GetRepository<Domain.Models.Task, string>();

            var task = _mapper.Map<Domain.Models.Task>(taskCreationDto);

            task.Id = Guid.NewGuid().ToString();
            task.CreatedOn = DateTime.UtcNow;
            task.LastUpdatedAt = DateTime.UtcNow;

            ///////////////////////Temp/////////////////////////
            task.AssignedById = 3;
            task.TeamId = "1";
            ///////////////////////////////////////////////////

            repo.Add(task);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TaskDetailedResponse>(task);
        }

        public async Task DeleteTaskAsync(string id)
        {

            //make sure that the user is able to delete this task

            var repo = _unitOfWork.GetRepository<Domain.Models.Task, string>();

            var specs = new TaskSpecifications(id);

            var task = await repo.GetByIdAsync(specs);

            if (task == null || task.IsDeleted == true)
                throw new TaskNotFoundException(id);

            task.IsDeleted = true;
            repo.Update(task);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskResponse>> GetAllTasksAsync(TaskQueryParameters taskQueryParameters) //specs
        {
            var specs = new TaskSpecifications(taskQueryParameters);

            var TasksRepo = _unitOfWork.GetRepository<Domain.Models.Task, string>();

            var tasks = await TasksRepo.GetAllAsync(specs);

            return _mapper.Map<IEnumerable<TaskResponse>>(tasks);

        }

        public async Task<TaskDetailedResponse> GetTaskByIdAsync(string id) //specs
        {

            var specs = new TaskSpecifications(id);

            var repo = _unitOfWork.GetRepository<Domain.Models.Task, string>();

            var task = await repo.GetByIdAsync(specs);

            return task == null ? throw new TaskNotFoundException(id) : _mapper.Map<TaskDetailedResponse>(task);
        }

        public async Task<TaskDetailedResponse> UpdateTaskAsync(TaskUpdateDto taskUpdateDto)
        {

            //make sure that the user is able to update this task

            var repo = _unitOfWork.GetRepository<Domain.Models.Task, string>();

            var specs = new TaskSpecifications(taskUpdateDto.Id);

            var existingTask = await repo.GetByIdAsync(specs) 
                ?? throw new TaskNotFoundException(taskUpdateDto.Id);

            _mapper.Map(taskUpdateDto, existingTask);

            existingTask.LastUpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TaskDetailedResponse>(existingTask);

        }

    }
}
