﻿using Fr8.Infrastructure.Data.DataTransferObjects;

namespace HubWeb.Documentation.Swagger
{
    public class FilterConditionSampleFactory : ISwaggerSampleFactory<FilterConditionDTO>
    {
        public FilterConditionDTO GetSampleData()
        {
            return new FilterConditionDTO
            {
                Value = "John",
                Field = "First Name",
                Operator = "eq"
            };
        }

        object ISwaggerSampleFactory.GetSampleData()
        {
            return GetSampleData();
        }
    }
}