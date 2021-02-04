﻿// <copyright file="LearningModuleMapperTests.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>
namespace Microsoft.Teams.Apps.LearnNow.Tests.Mappers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Teams.Apps.LearnNow.Infrastructure.Models;
    using Microsoft.Teams.Apps.LearnNow.Infrastructure.Repositories;
    using Microsoft.Teams.Apps.LearnNow.ModelMappers;
    using Microsoft.Teams.Apps.LearnNow.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The LearningModuleMapperTests contains all the test cases for the Learning Module mapper.
    /// </summary>
    [TestClass]
    public class LearningModuleMapperTests
    {
        private Mock<LearnNowContext> learnNowDbContext;
        private LearningModuleMapper learningModuleMapper;
        private LearningModule learningModule;
        private LearningModuleViewModel learningModuleViewModel;
        private UserDetail userDetail;
        private List<UserDetail> userDetails;

        /// <summary>
        /// Initialize all test variables.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.learnNowDbContext = new Mock<LearnNowContext>();
            this.learningModuleMapper = new LearningModuleMapper(
                this.learnNowDbContext.Object);
            var moduleTagss = new List<LearningModuleTag>();
            var moduleTag = new LearningModuleTag()
            {
                TagId = Guid.NewGuid(),
                LearningModuleId = Guid.NewGuid(),
            };
            moduleTagss.Add(moduleTag);
            this.learningModule = new LearningModule
            {
                Id = Guid.NewGuid(),
                Title = "Test LM",
                Description = "Test LM",
                GradeId = Guid.NewGuid(),
                SubjectId = Guid.NewGuid(),
                Subject = new Subject { Id = Guid.NewGuid() },
                Grade = new Grade { Id = Guid.NewGuid() },
                ImageUrl = "https://img.com",
                CreatedBy = Guid.NewGuid(),
            };
            this.learningModuleViewModel = new LearningModuleViewModel
            {
                Id = Guid.NewGuid(),
                Title = "Test LM",
                Description = "Test LM",
                GradeId = Guid.NewGuid(),
                SubjectId = Guid.NewGuid(),
                ImageUrl = "https://img.com",
                LearningModuleTag = moduleTagss,
            };
            this.userDetail = new UserDetail
            {
                UserId = Guid.NewGuid(),
                DisplayName = "User1",
            };
            this.userDetails = new List<UserDetail>
            {
                this.userDetail,
            };
        }

        /// <summary>
        /// Method for testing MapToDTO method of learning module mapper.
        /// </summary>
        [TestMethod]
        public void MapToDTOTest()
        {
            //// ARRANGE
            var learningModuleId = Guid.NewGuid();
            this.learningModuleViewModel.Id = learningModuleId;

            //// ACT
            var result = this.learningModuleMapper.MapToDTO(this.learningModuleViewModel, Guid.NewGuid());

            //// ASSERT
            Assert.AreEqual(result.Id, this.learningModuleViewModel.Id);
            Assert.AreEqual(result.Title, this.learningModuleViewModel.Title);
            Assert.AreEqual(result.GradeId, this.learningModuleViewModel.GradeId);
            Assert.AreEqual(result.ImageUrl, this.learningModuleViewModel.ImageUrl);
        }

        /// <summary>
        /// Method for testing MapToDTO method from mapper.
        /// </summary>
        [TestMethod]
        public void CanGetMapToViewModel()
        {
            //// ARRANGE
            var learningModuleId = Guid.NewGuid();
            this.learningModule.Id = learningModuleId;
            var userDetail = new UserDetail
            {
                UserId = this.learningModule.UpdatedBy,
                DisplayName = "User2",
            };
            this.userDetails.Add(userDetail);
            //// ACT
            var result = this.learningModuleMapper.MapToViewModel(this.learningModule, this.userDetails);

            //// ASSERT
            Assert.AreEqual(result.Id, learningModuleId);
        }

        /// <summary>
        /// Method for testing PatchAndMapToDTO method from mapper.
        /// </summary>
        [TestMethod]
        public void CanPatchAndMapToDTO()
        {
            //// ARRANGE
            var learningModuleUpdatedBy = Guid.NewGuid();
            this.learningModuleViewModel.UpdatedBy = learningModuleUpdatedBy;
            //// ACT
            var result = this.learningModuleMapper.PatchAndMapToDTO(this.learningModuleViewModel, learningModuleUpdatedBy);

            //// ASSERT
            Assert.AreEqual(result.UpdatedBy, learningModuleUpdatedBy);
        }

        /// <summary>
        /// Method for testing PatchAndMapToViewModel method from mapper.
        /// </summary>
        [TestMethod]
        public void CanPatchAndMapToViewModel()
        {
            //// ARRANGE
            var learningModuleCreatedBy = Guid.NewGuid();
            var userAadObjectId = Guid.NewGuid();
            this.learningModule.CreatedBy = learningModuleCreatedBy;
            var learningModuleVote = new LearningModuleVote
            {
                Id = Guid.NewGuid(),
                UserId = userAadObjectId,
            };
            List<LearningModuleVote> learningModuleVotes = new List<LearningModuleVote>();
            learningModuleVotes.Add(learningModuleVote);
            var userDetail = new UserDetail
            {
                UserId = this.learningModule.CreatedBy,
                DisplayName = "User3",
            };
            this.userDetails.Add(userDetail);
            //// ACT
            var result = this.learningModuleMapper.PatchAndMapToViewModel(this.learningModule, userAadObjectId, learningModuleVotes, 2, this.userDetails);

            //// ASSERT
            Assert.AreEqual(result.CreatedBy, learningModuleCreatedBy);
        }
    }
}