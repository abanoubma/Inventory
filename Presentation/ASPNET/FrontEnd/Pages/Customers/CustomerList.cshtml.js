const App = {
    setup() {
        const state = Vue.reactive({
            mainData: [],
            mainTitle: 'Add Customer',
            id: '',
            number: '',
            name: '',
            trn: '',
            customerGroupId: null,
            countryId: null,
            governorateId: null,
            cityId: null,
            buildingNumber: '',
            floor: '',
            flatNumber: '',
            postalCode: '',
            street: '',
            mobile: '',

            customerGroups: [],
            countries: [],
            governorates: [],
            cities: [],

            errors: {
                name: '',
                customerGroupId: '',
                countryId: '',
                governorateId: '',
                cityId: '',
                mobile: ''
            },
            isSubmitting: false
        });

        const mainGridRef = Vue.ref(null);
        const mainModalRef = Vue.ref(null);
        const customerGroupIdRef = Vue.ref(null);
        const countryRef = Vue.ref(null);
        const governorateRef = Vue.ref(null);
        const cityRef = Vue.ref(null);

        const services = {
            getMainData: () => AxiosManager.get('/Customer/GetCustomerList'),
            createMainData: (payload) => AxiosManager.post('/Customer/CreateCustomer', payload),
            updateMainData: (payload) => AxiosManager.post('/Customer/UpdateCustomer', payload),
            getCustomerGroups: () => AxiosManager.get('/CustomerGroup/GetCustomerGroupList'),
            getCountries: () => AxiosManager.get('/Location/GetCountries'),
            getGovernorates: (countryId) => AxiosManager.get(`/Location/GetGovernorates?countryId=${countryId}`),
            getCities: (governorateId) => AxiosManager.get(`/Location/GetCities?governorateId=${governorateId}`)
        };

        const customerGroupDropDown = {
            obj: null,
            create: () => {
                customerGroupDropDown.obj = new ej.dropdowns.DropDownList({
                    dataSource: state.customerGroups,
                    fields: { value: 'id', text: 'name' },
                    placeholder: 'Select Customer Group',
                    value: state.customerGroupId,
                    change: (e) => {
                        state.customerGroupId = e.value;
                        state.errors.customerGroupId = '';
                    }
                });
                customerGroupDropDown.obj.appendTo(customerGroupIdRef.value);
            },
            refresh: () => {
                if (customerGroupDropDown.obj) {
                    customerGroupDropDown.obj.value = state.customerGroupId;
                }
            }
        };

        const countryDropDown = {
            obj: null,
            create: async () => {
                const res = await services.getCountries();
                state.countries = res?.data?.content?.data || [];
                countryDropDown.obj = new ej.dropdowns.DropDownList({
                    dataSource: state.countries,
                    fields: { value: 'id', text: 'name' },
                    placeholder: 'Select Country',
                    value: state.countryId,
                    change: async (e) => {
                        state.countryId = e.value;
                        state.governorateId = null;
                        state.cityId = null;
                        state.errors.countryId = '';
                        if (e.value) {
                            const res = await services.getGovernorates(e.value);
                            state.governorates = res?.data?.content?.data || [];
                            governorateDropDown.obj.dataSource = state.governorates;
                            governorateDropDown.obj.value = null;
                            cityDropDown.obj.dataSource = [];
                            cityDropDown.obj.value = null;
                        }
                    }
                });
                countryDropDown.obj.appendTo(countryRef.value);
            }
        };

        const governorateDropDown = {
            obj: null,
            create: () => {
                governorateDropDown.obj = new ej.dropdowns.DropDownList({
                    dataSource: state.governorates,
                    fields: { value: 'id', text: 'name' },
                    placeholder: 'Select Governorate',
                    value: state.governorateId,
                    change: async (e) => {
                        state.governorateId = e.value;
                        state.cityId = null;
                        state.errors.governorateId = '';
                        if (e.value) {
                            const res = await services.getCities(e.value);
                            state.cities = res?.data?.content?.data || [];
                            cityDropDown.obj.dataSource = state.cities;
                            cityDropDown.obj.value = null;
                        }
                    }
                });
                governorateDropDown.obj.appendTo(governorateRef.value);
            }
        };

        const cityDropDown = {
            obj: null,
            create: () => {
                cityDropDown.obj = new ej.dropdowns.DropDownList({
                    dataSource: state.cities,
                    fields: { value: 'id', text: 'name' },
                    placeholder: 'Select City',
                    value: state.cityId,
                    change: (e) => {
                        state.cityId = e.value;
                        state.errors.cityId = '';
                    }
                });
                cityDropDown.obj.appendTo(cityRef.value);
            }
        };

        const mainGrid = {
            obj: null,
            create: (data) => {
                mainGrid.obj = new ej.grids.Grid({
                    dataSource: data,
                    height: '600px',
                    allowPaging: true,
                    allowSorting: true,
                    allowFiltering: true,
                    allowSelection: true,
                    allowResizing: true,
                    allowExcelExport: true,
                    filterSettings: { type: 'CheckBox' },
                    pageSettings: { pageSize: 20, pageSizes: [10, 20, 50, 100, 'All'] },
                    selectionSettings: { type: 'Single', persistSelection: true },
                    toolbar: [
                        'ExcelExport',
                        'Search',
                        { text: 'Add', tooltipText: 'Add New Customer', prefixIcon: 'e-add', id: 'addBtn' },
                        { text: 'Edit', tooltipText: 'Edit Selected Customer', prefixIcon: 'e-edit', id: 'editBtn' },
                        { text: 'Delete', tooltipText: 'Delete Selected Customer', prefixIcon: 'e-delete', id: 'deleteBtn' }
                    ],
                    columns: [
                        { type: 'checkbox', width: 50 },
                        { field: 'id', isPrimaryKey: true, visible: false },
                        { field: 'number', headerText: 'Number', width: 120 },
                        { field: 'name', headerText: 'Name', width: 220 },
                        { field: 'customerGroupName', headerText: 'Group', width: 150 },
                        { field: 'trn', headerText: 'TRN', width: 140 },
                        { field: 'mobile', headerText: 'Mobile', width: 140 },
                        { field: 'cityName', headerText: 'City', width: 140 },
                        { field: 'street', headerText: 'Street', width: 180 },
                        { field: 'createdAtUtc', headerText: 'Created', width: 150, format: 'yMd' }
                    ],
                    rowSelected: () => {
                        const selected = mainGrid.obj.getSelectedRecords().length > 0;
                        mainGrid.obj.toolbarModule.enableItems(['editBtn', 'deleteBtn'], selected);
                    },
                    rowDeselected: () => {
                        mainGrid.obj.toolbarModule.enableItems(['editBtn', 'deleteBtn'], false);
                    },
                    toolbarClick: async (args) => {
                        // Excel Export
                        if (args.item.id === mainGrid.obj.element.id + '_excelexport') {
                            mainGrid.obj.excelExport();
                            return;
                        }

                        switch (args.item.id) {
                            case 'addBtn':
                                resetForm();                     // ← Fixed: was "Form()"
                                state.mainTitle = 'Add Customer';
                                mainModal.obj.show();
                                break;

                            case 'editBtn':
                                const selectedRow = mainGrid.obj.getSelectedRecords()[0];
                                if (selectedRow) {
                                    await loadForm(selectedRow); // ← loadForm already refreshes dropdowns
                                    state.mainTitle = 'Edit Customer';
                                    mainModal.obj.show();
                                }
                                break;

                            case 'deleteBtn':
                                const rowToDelete = mainGrid.obj.getSelectedRecords()[0];
                                if (!rowToDelete) return;

                                const confirmResult = await Swal.fire({
                                    title: 'Delete Customer?',
                                    text: `Are you sure you want to delete "${rowToDelete.name}"?`,
                                    icon: 'warning',
                                    showCancelButton: true,
                                    confirmButtonText: 'Yes, delete',
                                    cancelButtonText: 'Cancel'
                                });

                                if (confirmResult.isConfirmed) {
                                    try {
                                        const result = await AxiosManager.post('/Customer/DeleteCustomer', {
                                            id: rowToDelete.id
                                        });

                                        if (result.data.code === 200) {
                                            await refreshGrid();
                                            Swal.fire('Deleted!', 'Customer has been deleted.', 'success');
                                        } else {
                                            Swal.fire('Error', result.data.message || 'Delete failed', 'error');
                                        }
                                    } catch (err) {
                                        console.error(err);
                                        Swal.fire('Error', 'Failed to delete customer', 'error');
                                    }
                                }
                                break;
                        }
                    },
                    dataBound: () => {
                        mainGrid.obj.autoFitColumns();
                        mainGrid.obj.toolbarModule.enableItems(['editBtn', 'deleteBtn'], false);
                    }
                });

                mainGrid.obj.appendTo(mainGridRef.value);
            }
        };

        const mainModal = {
            obj: null,
            create: () => {
                mainModal.obj = new bootstrap.Modal(mainModalRef.value);
            }
        };

        const resetForm = () => {
            Object.keys(state.errors).forEach(k => state.errors[k] = '');
            state.id = '';
            state.number = '';
            state.name = '';
            state.trn = '';
            state.customerGroupId = null;
            state.countryId = null;
            state.governorateId = null;
            state.cityId = null;
            state.buildingNumber = '';
            state.floor = '';
            state.flatNumber = '';
            state.postalCode = '';
            state.street = '';
            state.mobile = '';

            // Reset dropdowns
            if (customerGroupDropDown.obj) customerGroupDropDown.obj.value = null;
            if (countryDropDown.obj) {
                countryDropDown.obj.value = null;
                countryDropDown.obj.dataSource = state.countries;
            }
            if (governorateDropDown.obj) {
                governorateDropDown.obj.value = null;
                governorateDropDown.obj.dataSource = [];
            }
            if (cityDropDown.obj) {
                cityDropDown.obj.value = null;
                cityDropDown.obj.dataSource = [];
            }
        };

        const loadForm = async (row) => {
            state.id = row.id || '';
            state.number = row.number || '';
            state.name = row.name || '';
            state.trn = row.trn || '';
            state.customerGroupId = row.customerGroupId || null;
            state.mobile = row.mobile || '';
            state.buildingNumber = row.buildingNumber || '';
            state.floor = row.floor || '';
            state.flatNumber = row.flatNumber || '';
            state.postalCode = row.postalCode || '';
            state.street = row.street || '';

            // Location fields
            state.countryId = row.countryId || null;
            state.governorateId = row.governorateId || null;
            state.cityId = row.cityId || null;

            // Refresh customer group
            customerGroupDropDown.refresh();

            // Re-populate cascading dropdowns
            if (state.countryId) {
                const govRes = await services.getGovernorates(state.countryId);
                state.governorates = govRes?.data?.content?.data || [];
                governorateDropDown.obj.dataSource = state.governorates;
                governorateDropDown.obj.value = state.governorateId;

                if (state.governorateId) {
                    const cityRes = await services.getCities(state.governorateId);
                    state.cities = cityRes?.data?.content?.data || [];
                    cityDropDown.obj.dataSource = state.cities;
                    cityDropDown.obj.value = state.cityId;
                }
            }
        };

        const handler = {
            handleSubmit: async () => {
                state.errors = { name: '', customerGroupId: '', countryId: '', governorateId: '', cityId: '', mobile: '' };
                let valid = true;

                if (!state.name) { state.errors.name = 'Required'; valid = false; }
                if (!state.customerGroupId) { state.errors.customerGroupId = 'Required'; valid = false; }
                if (!state.countryId) { state.errors.countryId = 'Required'; valid = false; }
                if (!state.governorateId) { state.errors.governorateId = 'Required'; valid = false; }
                if (!state.cityId) { state.errors.cityId = 'Required'; valid = false; }

                if (!valid) return;

                state.isSubmitting = true;
                try {
                    const payload = {
                        id: state.id,
                        name: state.name,
                        trn: state.trn,
                        customerGroupId: state.customerGroupId,
                        countryId: state.countryId,
                        governorateId: state.governorateId,
                        cityId: state.cityId,
                        buildingNumber: state.buildingNumber,
                        floor: state.floor,
                        flatNumber: state.flatNumber,
                        postalCode: state.postalCode,
                        street: state.street,
                        mobile: state.mobile
                    };

                    const res = state.id
                        ? await services.updateMainData(payload)
                        : await services.createMainData(payload);

                    if (res.data.code === 200) {
                        await refreshGrid();
                        Swal.fire('Success', 'Customer saved successfully', 'success');
                        mainModal.obj.hide();
                    } else {
                        Swal.fire('Error', res.data.message || 'Save failed', 'error');
                    }
                } catch (err) {
                    Swal.fire('Error', 'An error occurred', 'error');
                } finally {
                    state.isSubmitting = false;
                }
            }
        };

        const refreshGrid = async () => {
            const res = await services.getMainData();
            state.mainData = res?.data?.content?.data || [];
            mainGrid.obj.dataSource = state.mainData;
        };

        Vue.onMounted(async () => {
            try {
                await SecurityManager.authorizePage(['Customers']);
                await SecurityManager.validateToken();

                const [mainRes, groupRes] = await Promise.all([
                    services.getMainData(),
                    services.getCustomerGroups()
                ]);

                state.mainData = mainRes?.data?.content?.data || [];
                state.customerGroups = groupRes?.data?.content?.data || [];

                mainGrid.create(state.mainData);
                customerGroupDropDown.create();
                countryDropDown.create();
                governorateDropDown.create();
                cityDropDown.create();
                mainModal.create();
            } catch (e) {
                console.error('Page init error:', e);
            }
        });

        return {
            state,
            mainGridRef,
            mainModalRef,
            customerGroupIdRef,
            countryRef,
            governorateRef,
            cityRef,
            handler
        };
    }
};

Vue.createApp(App).mount('#app');