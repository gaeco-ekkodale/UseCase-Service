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
import type { CreateUseCaseRequest } from '../models/CreateUseCaseRequest';
import type { UpdateUseCaseRequest } from '../models/UpdateUseCaseRequest';
import type { UseCaseDto } from '../models/UseCaseDto';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class UsecasesService {
    /**
     * Create a new use case
     * Creates a new use case and returns its data
     * @param requestBody
     * @returns UseCaseDto Success
     * @throws ApiError
     */
    public static createUseCaseEndpoint(
        requestBody: CreateUseCaseRequest,
    ): CancelablePromise<UseCaseDto> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/usecases',
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad Request`,
                401: `Unauthorized`,
                403: `Forbidden`,
            },
        });
    }
    /**
     * Get all use cases
     * Returns all use cases
     * @returns UseCaseDto Success
     * @throws ApiError
     */
    public static getAllUseCasesEndpoint(): CancelablePromise<Array<UseCaseDto>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/usecases',
            errors: {
                400: `Bad Request`,
                401: `Unauthorized`,
                403: `Forbidden`,
            },
        });
    }
    /**
     * Delete use case
     * Deletes an existing use case
     * @param id
     * @returns any Success
     * @throws ApiError
     */
    public static deleteUseCaseEndpoint(
        id: string,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/usecases/{id}',
            path: {
                'id': id,
            },
            errors: {
                400: `Bad Request`,
                401: `Unauthorized`,
                403: `Forbidden`,
                404: `Not Found`,
            },
        });
    }
    /**
     * Get use case by id
     * Returns a single use case
     * @param id
     * @returns UseCaseDto Success
     * @throws ApiError
     */
    public static getUseCaseByIdEndpoint(
        id: string,
    ): CancelablePromise<UseCaseDto> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/usecases/{id}',
            path: {
                'id': id,
            },
            errors: {
                400: `Bad Request`,
                401: `Unauthorized`,
                403: `Forbidden`,
                404: `Not Found`,
            },
        });
    }
    /**
     * Update use case
     * Updates an existing use case
     * @param id
     * @param requestBody
     * @returns any Success
     * @throws ApiError
     */
    public static updateUseCaseEndpoint(
        id: string,
        requestBody: UpdateUseCaseRequest,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/usecases/{id}',
            path: {
                'id': id,
            },
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad Request`,
                401: `Unauthorized`,
                403: `Forbidden`,
                404: `Not Found`,
            },
        });
    }
}
