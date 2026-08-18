// Copyright (c) 2025 Ekkodale GmbH. All rights reserved.
//
// This file is part of the gaeco platform system.
//
// Use of this file is governed by the terms of the license
// in LICENSE.md at the root of this repository.
// Unauthorized copying, modification, distribution, or use of this file,
// via any medium, is strictly prohibited except as expressly permitted
// under that license.

import { OpenAPI } from "./api/UseCaseService/core/OpenAPI";
import { useAuth } from "react-oidc-context";
import { Route, Routes } from "react-router-dom";
import { useEffect } from "react";
import { ThemeProvider } from "@emotion/react";
import { CssBaseline } from "@mui/material";
import { Toaster } from "sonner";
import OverviewPanel from "./features/UseCaseOverview/components/OverviewPanel";
import { baseTheme } from "./styles/muiThemes";
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import './index.css';

// Query Client setzen
const queryClient = new QueryClient()

// API-Basis-URL setzen
OpenAPI.BASE = import.meta.env.VITE_API_URL;

function App() {
  const auth = useAuth();
  OpenAPI.TOKEN = auth.user?.access_token;

  useEffect(() => {
    if (auth.user?.access_token) {
      OpenAPI.TOKEN = auth.user.access_token;
    } else {
      OpenAPI.TOKEN = undefined;
    }
  }, [auth]);

  return (
    <div>
      <Routes>
        <Route path="/*" element={
          <ThemeProvider theme={baseTheme}>
            <CssBaseline />
            <Toaster richColors />
            <QueryClientProvider client={queryClient}>
              <OverviewPanel />
            </QueryClientProvider>
          </ThemeProvider>} />
      </Routes>
    </div>
  );
}

export default App;
