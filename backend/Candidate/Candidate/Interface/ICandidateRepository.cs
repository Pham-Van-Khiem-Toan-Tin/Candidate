﻿using Candidate.DTOs;
using Candidate.Form;

namespace Candidate.Interface
{
    public interface ICandidateRepository
    {
        Task<bool> CreateCandidate(CandidateCreateForm candidateCreateForm, IFormFile file);
        Task<List<CandidateDTO>> GetAllCandidate();
    }
}
