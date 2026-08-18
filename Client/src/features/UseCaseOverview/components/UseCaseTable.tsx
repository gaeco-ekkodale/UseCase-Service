// Copyright (c) 2025 Ekkodale GmbH. All rights reserved.
//
// This file is part of the gaeco platform system.
//
// Use of this file is governed by the terms of the license
// in LICENSE.md at the root of this repository.
// Unauthorized copying, modification, distribution, or use of this file,
// via any medium, is strictly prohibited except as expressly permitted
// under that license.

import AddIcon from "@mui/icons-material/Add";
import {Box, Button, CircularProgress, IconButton, Modal, TextField, Tooltip, Typography} from "@mui/material";
import {MaterialReactTable, type MRT_ColumnDef, MRT_ToggleGlobalFilterButton, MRT_ToggleFiltersButton, MRT_ShowHideColumnsButton, MRT_ToggleDensePaddingButton, MRT_ToggleFullScreenButton} from "material-react-table";
import {useEffect, useMemo, useState} from "react";
import {toast} from "sonner";
import {UseCaseDto} from "../../../api/UseCaseService";
import EmptyState from "../../../components/EmptyState";
import Tour from "../../tour/Tour";
import {TOUR_KEY, TOUR_MODULE_NAME, TOUR_PANELS} from "../../tour/tourContent";
import {useCreateUseCase} from "../hooks/useCreateUseCase";
import {useGetUseCases} from "../hooks/useGetUseCases";
import {useUpdateUseCase} from "../hooks/useUpdateUseCase";

export default function UseCaseTable() {
  // isPending, not isLoading: isLoading is `isPending && isFetching`, and on the very
  // first render the fetch has not started yet, so it is briefly false while there is
  // still no data - long enough to flash the empty state before the table appears.
  const {isPending, isError, data: fetchedData} = useGetUseCases();
  const {mutate: updateUseCase} = useUpdateUseCase();
  const {mutate: addNewUseCase} = useCreateUseCase();
  const [data, setData] = useState<UseCaseDto[]>([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [newUseCase, setNewUseCase] = useState<Partial<UseCaseDto>>({name: "", description: ""});
  const validateRequired = (value: string) => !!value.length;

  useEffect(() => {
    if (fetchedData) {
      setData(fetchedData);
    }
  }, [fetchedData]);

  /**
   * Creates a column structure for the table.
   */
  const columns = useMemo<MRT_ColumnDef<UseCaseDto>[]>(
    () => [
      {
        accessorKey: "name",
        header: "Name",
        enableHiding: false,
        muiEditTextFieldProps: ({row, cell}) => ({
          type: "text",
          required: true,
          onBlur: (event) => {
            const validationError = !validateRequired(event.currentTarget.value)
              ? "Required"
              : undefined;
            if (validationError) {
              toast.error(validationError);
              return;
            } else if(event.currentTarget.value === cell.getValue()) {
              toast.info("No changes to apply");
              return;
            }
            const updatedRow = {...row.original, name: event.currentTarget.value};
            updateUseCase({
              id: updatedRow.id || "",
              name: updatedRow.name || "",
              description: updatedRow.description || "",
            });
          },
        }),
      },
      {
        accessorKey: "description",
        header: "Description",
        muiEditTextFieldProps: ({row, cell}) => ({
          type: "text",
          required: true,
          onBlur: (event) => {
            const validationError = !validateRequired(event.currentTarget.value)
              ? "Required"
              : undefined;
            if (validationError) {
              toast.error(validationError);
              return;
            } else if(event.currentTarget.value === cell.getValue()) {
              toast.info("No changes to apply");
              return;
            }
            const updatedRow = {...row.original, description: event.currentTarget.value};
            updateUseCase({
              id: updatedRow.id || "",
              name: updatedRow.name || "",
              description: updatedRow.description || "",
            });
          },
        }),
      },
    ],
    [updateUseCase]
  );

  /**
   * Handles the creation of a new use case. Checks if all information is provided.
   */
  const handleCreateNewUseCase = () => {
    // Ensure all required fields are filled out
    if (!newUseCase.name || !newUseCase.description) {
      toast.error("Please fill out all fields.");
      return;
    }
    newUseCase.id = "string";

    // Trigger the mutation to add the new use case
    addNewUseCase({name: newUseCase.name, description: newUseCase.description});
    handleCloseModal();
  };

  /**
   * Opens the "Create Use Case" modal.
   */
  const handleOpenModal = () => {
    setIsModalOpen(true);
  };

  /**
   * Closes "Create Use Case" modal.
   */
  const handleCloseModal = () => {
    setIsModalOpen(false);
    setNewUseCase({name: "", description: ""});
  };

  /**
   * Renders the table, or the state explaining why there is no table to show.
   * Loading, failure and "nothing created yet" are deliberately distinct: a
   * first-time user needs to know which of the three they are looking at.
   */
  const renderContent = () => {
    if (isPending) {
      return (
        <div className="flex h-full items-center justify-center p-8">
          <CircularProgress />
        </div>
      );
    }

    if (isError) {
      return (
        <EmptyState
          tone="error"
          title="Could not load UseCases"
          description="The list of UseCases could not be loaded."
          footnote="A connection problem, not missing setup. Check that the UseCase service is running, then reload."
        />
      );
    }

    // Checked against the query result, not the mirrored state, so the empty
    // state does not flash for one frame before the state effect catches up.
    if (!fetchedData || fetchedData.length === 0) {
      return (
        <EmptyState
          title="No UseCases yet"
          description="A UseCase is the working context data is viewed and edited from."
          action={
            <Button variant="contained" startIcon={<AddIcon />} onClick={handleOpenModal}>
              Create UseCase
            </Button>
          }
        />
      );
    }

    return (
      <MaterialReactTable<UseCaseDto>
        columns={columns}
        data={data}
        getRowId={(originalRow) => originalRow.id || ""}
        enableEditing
        editDisplayMode="cell"
        initialState={{sorting: [{id: "name", desc: false}]}}
        renderTopToolbarCustomActions={() => (
          <Typography variant="h1" component="div" className="!text-4xl !text-gray-600">
            UseCases
          </Typography>
        )}
        renderToolbarInternalActions={({table}) => (
          <>
            <MRT_ToggleGlobalFilterButton table={table} />
            <Tooltip title="Add UseCase">
              <IconButton aria-label="Add UseCase" onClick={handleOpenModal}>
                <AddIcon className="rounded-full text-white bg-blue-500" />
              </IconButton>
            </Tooltip>
            <MRT_ToggleFiltersButton table={table} />
            <MRT_ShowHideColumnsButton table={table} />
            <MRT_ToggleDensePaddingButton table={table} />
            <MRT_ToggleFullScreenButton table={table} />
          </>
        )}
      />
    );
  };

  return (
    <div className="h-full">
      {/* Fixed to the viewport corner, so it is reachable in every state. */}
      <Tour tourKey={TOUR_KEY} moduleName={TOUR_MODULE_NAME} panels={TOUR_PANELS} />
      {renderContent()}
      <Modal
        open={isModalOpen}
        onClose={handleCloseModal}
        aria-labelledby="modal-modal-name"
        aria-describedby="modal-modal-description"
      >
        <Box
          component="form"
          className="flex flex-col gap-4 absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 min-w-96 bg-white shadow-xl p-6"
        >
          <TextField
            label="Name"
            value={newUseCase.name}
            onChange={(e) => setNewUseCase({...newUseCase, name: e.target.value})}
            required
          />
          <TextField
            label="Description"
            value={newUseCase.description}
            onChange={(e) => setNewUseCase({...newUseCase, description: e.target.value})}
            multiline
            rows={4}
            required
          />
          <Button variant="contained" onClick={handleCreateNewUseCase}>
            Save
          </Button>
          <Button variant="outlined" onClick={handleCloseModal}>
            Close
          </Button>
        </Box>
      </Modal>
    </div>
  );
}
