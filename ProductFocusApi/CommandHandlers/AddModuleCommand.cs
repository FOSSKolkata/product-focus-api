﻿using CSharpFunctionalExtensions;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Threading.Tasks;
using MediatR;
using System.Threading;

namespace ProductFocus.AppServices
{
    public sealed class AddModuleCommand : IRequest<Result>
    {
        public long Id { get; }
        public string Name { get; }
        public AddModuleCommand(long id, string name)
        {
            Id = id;
            Name = name;
        }

        internal sealed class AddModuleCommandHandler : IRequestHandler<AddModuleCommand, Result>
        {
            private readonly IProductRepository _productRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IModuleRepository _moduleRepository;

            public AddModuleCommandHandler(
                IProductRepository productRepository,
                IUnitOfWork unitOfWork,
                IModuleRepository moduleRepository)
            {
                _productRepository = productRepository;
                _unitOfWork = unitOfWork;
                _moduleRepository = moduleRepository;
            }
            public async Task<Result> Handle(AddModuleCommand request, CancellationToken cancellationToken)
            {
                Product product = await _productRepository.GetById(request.Id);
                if (product == null)
                    return Result.Failure($"No product found with Id '{request.Id}'");
                Module module = await _moduleRepository.GetByName(request.Name);
                if (module != null)
                    return Result.Failure($"Module already exist with the name '{request.Name}'");
                try
                {
                    product.AddModule(request.Name);
                    await _unitOfWork.CompleteAsync(cancellationToken);
                    
                    return Result.Success();
                }
                catch(Exception ex)
                {
                    return Result.Failure(ex.Message);
                }
                
            }

        }
    }
}
