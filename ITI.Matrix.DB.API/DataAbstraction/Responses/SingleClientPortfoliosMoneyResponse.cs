﻿using DataAbstraction.Models;

namespace DataAbstraction.Responses
{
    public class SingleClientPortfoliosMoneyResponse
    {
        public ListStringResponseModel Response { get; set; } = new ListStringResponseModel();
        public List<PortfoliosAndMoneyModel> PortfoliosAndMoney { get; set; } = new List<PortfoliosAndMoneyModel>();
    }
}
