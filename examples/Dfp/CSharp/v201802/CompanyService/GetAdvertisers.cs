// Copyright 2017, Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Google.Api.Ads.Dfp.Lib;
using Google.Api.Ads.Dfp.Util.v201802;
using Google.Api.Ads.Dfp.v201802;

using System;

namespace Google.Api.Ads.Dfp.Examples.CSharp.v201802
{
    /// <summary>
    /// This example gets all companies that are advertisers.
    /// </summary>
    public class GetAdvertisers : SampleBase
    {
        /// <summary>
        /// Returns a description about the code example.
        /// </summary>
        public override string Description
        {
            get { return "This example gets all companies that are advertisers."; }
        }

        /// <summary>
        /// Main method, to run this code example as a standalone application.
        /// </summary>
        public static void Main()
        {
            GetAdvertisers codeExample = new GetAdvertisers();
            Console.WriteLine(codeExample.Description);
            try
            {
                codeExample.Run(new DfpUser());
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to get companies. Exception says \"{0}\"", e.Message);
            }
        }

        /// <summary>
        /// Run the code example.
        /// </summary>
        public void Run(DfpUser dfpUser)
        {
            using (CompanyService companyService =
                (CompanyService) dfpUser.GetService(DfpService.v201802.CompanyService))
            {
                // Create a statement to select companies.
                int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
                StatementBuilder statementBuilder = new StatementBuilder()
                    .Where("type = :type")
                    .OrderBy("id ASC")
                    .Limit(pageSize)
                    .AddValue("type", CompanyType.ADVERTISER.ToString());

                // Retrieve a small amount of companies at a time, paging through until all
                // companies have been retrieved.
                int totalResultSetSize = 0;
                do
                {
                    CompanyPage page =
                        companyService.getCompaniesByStatement(statementBuilder.ToStatement());

                    // Print out some information for each company.
                    if (page.results != null)
                    {
                        totalResultSetSize = page.totalResultSetSize;
                        int i = page.startIndex;
                        foreach (Company company in page.results)
                        {
                            Console.WriteLine(
                                "{0}) Company with ID {1}, name \"{2}\", and type \"{3}\" was " +
                                "found.",
                                i++, company.id, company.name, company.type);
                        }
                    }

                    statementBuilder.IncreaseOffsetBy(pageSize);
                } while (statementBuilder.GetOffset() < totalResultSetSize);

                Console.WriteLine("Number of results found: {0}", totalResultSetSize);
            }
        }
    }
}
