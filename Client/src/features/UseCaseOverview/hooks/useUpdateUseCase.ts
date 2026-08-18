// Copyright (c) 2025 Ekkodale GmbH. All rights reserved.
//
// This file is part of the gaeco platform system.
//
// Use of this file is governed by the terms of the license
// in LICENSE.md at the root of this repository.
// Unauthorized copying, modification, distribution, or use of this file,
// via any medium, is strictly prohibited except as expressly permitted
// under that license.

import {useMutation, useQueryClient} from "@tanstack/react-query";
import {toast} from "sonner";
import {UsecasesService} from "../../../api/UseCaseService";

/**
 * API call to update a use case.
 */
export const useUpdateUseCase = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({id, name, description}: {id: string; name: string; description: string}) =>
      UsecasesService.updateUseCaseEndpoint(id, {name, description}),
    onSuccess: () => {
      toast.success(`Use case was successfully modified.`);
      queryClient.invalidateQueries({queryKey: ["useCases"]});
    },
    onError: () => {
      toast.error("Something went wrong...");
    },
  });
};
