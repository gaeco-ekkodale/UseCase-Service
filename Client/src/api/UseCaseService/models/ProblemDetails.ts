// Copyright (c) 2025 Ekkodale GmbH. All rights reserved.
//
// This file is part of the gaeco platform system.
//
// Use of this file is governed by the terms of the license
// in LICENSE.md at the root of this repository.
// Unauthorized copying, modification, distribution, or use of this file,
// via any medium, is strictly prohibited except as expressly permitted
// under that license.

/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { ProblemDetails_Error } from './ProblemDetails_Error';
/**
 * RFC7807 compatible problem details/ error response class. this can be used by configuring startup like so:
 *
 * app.UseFastEndpoints(x => x.Errors.ResponseBuilder = ProblemDetails.ResponseBuilder);
 */
export type ProblemDetails = {
    type?: string;
    title?: string;
    status?: number;
    instance?: string;
    traceId?: string;
    /**
     * the details of the error
     */
    detail?: string | null;
    errors?: Array<ProblemDetails_Error>;
};

