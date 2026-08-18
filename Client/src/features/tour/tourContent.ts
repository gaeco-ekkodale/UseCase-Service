// Copyright (c) 2025 Ekkodale GmbH. All rights reserved.
//
// This file is part of the gaeco platform system.
//
// Use of this file is governed by the terms of the license
// in LICENSE.md at the root of this repository.
// Unauthorized copying, modification, distribution, or use of this file,
// via any medium, is strictly prohibited except as expressly permitted
// under that license.

import {TourPanel} from "./Tour";

export const TOUR_KEY = "usecase";
export const TOUR_MODULE_NAME = "UseCases";

/**
 * Describes this module and its place in gaeco - nothing beyond it. No pointers to other
 * modules or to tools outside the platform.
 *
 * Kept as data, not JSX, so the wording can be revised without touching a component.
 */
export const TOUR_PANELS: TourPanel[] = [
  {
    title: "The working context",
    body: "gaeco never shows simply all the data. Every view and every permission is bound to a UseCase: the working context you look at the data from.",
  },
  {
    title: "One data set, several perspectives",
    body: "Energy management and maintenance planning can address the same building and expose different parts of it. The building is stored once; the UseCase decides which section is relevant.",
  },
  {
    title: "Finding a good name",
    body: "Name a UseCase after the task it serves rather than after a department or a team — “Energy monitoring”, “Maintenance planning”, “Space management”. That name is what people choose from later, so it should say what the context is meant for.",
  },
  {
    title: "Creating and editing",
    body: "Choose + to add a UseCase with a name and a description. Select any cell in the table to edit it; the change applies when the field loses focus.",
  },
];
