// Copyright (c) 2025 Ekkodale GmbH. All rights reserved.
//
// This file is part of the gaeco platform system.
//
// Use of this file is governed by the terms of the license
// in LICENSE.md at the root of this repository.
// Unauthorized copying, modification, distribution, or use of this file,
// via any medium, is strictly prohibited except as expressly permitted
// under that license.

import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toast } from "sonner";
import { UsecasesService } from "../../../api/UseCaseService";

/**
 * Calls UseCaseService POST UseCaseDTO API.
 */
export const useCreateUseCase = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({name, description}: {name: string; description: string}) =>
      UsecasesService.createUseCaseEndpoint({name, description}),
    onSuccess: () => {
      toast.success("New use case was successfully added.");
      queryClient.invalidateQueries({queryKey: ["useCases"]});
    },
    onError: () => {
      toast.error("Something went wrong...");
    },
  });
};
