// Copyright (c) 2025 Ekkodale GmbH. All rights reserved.
//
// This file is part of the gaeco platform system.
//
// Use of this file is governed by the terms of the license
// in LICENSE.md at the root of this repository.
// Unauthorized copying, modification, distribution, or use of this file,
// via any medium, is strictly prohibited except as expressly permitted
// under that license.

import {useQuery} from "@tanstack/react-query";
import {UsecasesService} from "../../../api/UseCaseService";

/**
 * API call to retrieve UseCaseDTO's.
 * @returns UseCaseDTO[]
 */
export const useGetUseCases = () => {
  return useQuery({
    queryKey: ["useCases"],
    queryFn: () => UsecasesService.getAllUseCasesEndpoint(),
  });
};
