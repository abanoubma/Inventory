const App = {
    setup() {
        const state = Vue.reactive({
            mainData: [],
            mainTitle: 'Add Vendor',
            id: '',
            number: '',
            name: '',
            trn: '',
            vendorGroupId: null,
            countryId: null,
            governorateId: null,
            cityId: null,
            buildingNumber: '',
            floor: '',
            flatNumber: '',
            postalCode: '',
            street: '',
            mobile: '',

            vendorGroups: [],
            countries: [],
            governorates: [],
            cities: [],

            errors: {
                name: '',
                vendorGroupId: '',
                countryId: '',
                governorateId: '',
                cityId: '',
                mobile: ''
            },
            isSubmitting: false
        });

        const mainGridRef = Vue.ref(null);
        const mainModalRef = Vue.ref(null);
        const vendorGroupIdRef = Vue.ref(null);
        const countryRef = Vue.ref(null);
        const governorateRef = Vue.ref(null);
        const cityRef = Vue.ref(null);
        const nameRef = Vue.ref(null);
        const numberRef = Vue.ref(null);
        const trnRef = Vue.ref(null);
        const buildingRef = Vue.ref(null);
        const floorRef = Vue.ref(null);
        const flatRef = Vue.ref(null);
        const postalRef = Vue.ref(null);
        const streetRef = Vue.ref(null);
        const mobileRef = Vue.ref(null);

        const services = {
            getMainData: () => AxiosManager.get('/Vendor/GetVendorList'),
            createMainData: (payload) => AxiosManager.post('/Vendor/CreateVendor', payload, { headers: { 'Content-Type': 'application/json' } }),
            updateMainData: (payload) => AxiosManager.post('/Vendor/UpdateVendor', payload),
            getVendorGroups: () => AxiosManager.get('/VendorGroup/GetVendorGroupList'),
            getCountries: () => AxiosManager.get('/Location/GetCountries'),
            getGovernorates: (countryId) => AxiosManager.get(`/Location/GetGovernorates?countryId=${countryId}`),
            getCities: (governorateId) => AxiosManager.get(`/Location/GetCities?governorateId=${governorateId}`)
        };

        const vendorGroupDropDown = {
            obj: null,
            create: () => {
                vendorGroupDropDown.obj = new ej.dropdowns.DropDownList({
                    dataSource: state.vendorGroups,
                    fields: { value: 'id', text: 'name' },
                    placeholder: 'Select Vendor Group',
                    value: state.vendorGroupId,
                    change: (e) => {
                        state.vendorGroupId = e.value;
                        state.errors.vendorGroupId = '';
                    }
                });
                vendorGroupDropDown.obj.appendTo(vendorGroupIdRef.value);
            },
            refresh: () => {
                if (vendorGroupDropDown.obj) {
                    vendorGroupDropDown.obj.value = state.vendorGroupId;
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
            },
            refresh: () => {
                if (governorateDropDown.obj) governorateDropDown.obj.value = state.governorateId;
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
            },
            refresh: () => {
                if (cityDropDown.obj) cityDropDown.obj.value = state.cityId;
            }
        };

        const nameText = { obj: null, create: () => { nameText.obj = new ej.inputs.TextBox({ placeholder: 'Enter Vendor Name' }); nameText.obj.appendTo(nameRef.value); }, refresh: () => { if (nameText.obj) nameText.obj.value = state.name; } };
        const numberText = { obj: null, create: () => { numberText.obj = new ej.inputs.TextBox({ placeholder: '[auto]', readonly: true }); numberText.obj.appendTo(numberRef.value); }, refresh: () => { if (numberText.obj) numberText.obj.value = state.number; } };
        const trnText = { obj: null, create: () => { trnText.obj = new ej.inputs.TextBox({ placeholder: 'Enter TRN' }); trnText.obj.appendTo(trnRef.value); }, refresh: () => { if (trnText.obj) trnText.obj.value = state.trn; } };

        const buildingText = { obj: null, create: () => { buildingText.obj = new ej.inputs.TextBox({ placeholder: 'Building Number' }); buildingText.obj.appendTo(buildingRef.value); }, refresh: () => { if (buildingText.obj) buildingText.obj.value = state.buildingNumber; } };
        const floorText = { obj: null, create: () => { floorText.obj = new ej.inputs.TextBox({ placeholder: 'Floor' }); floorText.obj.appendTo(floorRef.value); }, refresh: () => { if (floorText.obj) floorText.obj.value = state.floor; } };
        const flatText = { obj: null, create: () => { flatText.obj = new ej.inputs.TextBox({ placeholder: 'Flat Number' }); flatText.obj.appendTo(flatRef.value); }, refresh: () => { if (flatText.obj) flatText.obj.value = state.flatNumber; } };
        const streetText = { obj: null, create: () => { streetText.obj = new ej.inputs.TextBox({ placeholder: 'Street' }); streetText.obj.appendTo(streetRef.value); }, refresh: () => { if (streetText.obj) streetText.obj.value = state.street; } };
        const postalText = { obj: null, create: () => { postalText.obj = new ej.inputs.TextBox({ placeholder: 'Postal Code' }); postalText.obj.appendTo(postalRef.value); }, refresh: () => { if (postalText.obj) postalText.obj.value = state.postalCode; } };
        const mobileText = { obj: null, create: () => { mobileText.obj = new ej.inputs.TextBox({ placeholder: 'Mobile Number' }); mobileText.obj.appendTo(mobileRef.value); }, refresh: () => { if (mobileText.obj) mobileText.obj.value = state.mobile; } };

        Vue.watch(() => state.name, () => { state.errors.name = ''; nameText.refresh(); });
        Vue.watch(() => state.vendorGroupId, () => { state.errors.vendorGroupId = ''; vendorGroupDropDown.refresh(); });
        Vue.watch(() => state.countryId, () => { state.errors.countryId = ''; if (countryDropDown.obj) countryDropDown.obj.value = state.countryId; });
        Vue.watch(() => state.governorateId, () => { state.errors.governorateId = ''; if (governorateDropDown.obj) governorateDropDown.obj.value = state.governorateId; });
        Vue.watch(() => state.cityId, () => { state.errors.cityId = ''; if (cityDropDown.obj) cityDropDown.obj.value = state.cityId; });
        Vue.watch(() => state.mobile, () => { state.errors.mobile = ''; mobileText.refresh(); });

        const handler = {
            handleSubmit: async () => {
                state.errors = { name: '', vendorGroupId: '', countryId: '', governorateId: '', cityId: '', mobile: '' };
                let valid = true;

                if (!state.name) { state.errors.name = 'Required'; valid = false; }
                if (!state.vendorGroupId) { state.errors.vendorGroupId = 'Required'; valid = false; }
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
                        vendorGroupId: state.vendorGroupId,
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
                        Swal.fire('Success', 'Vendor saved successfully', 'success');
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
                        { text: 'Add', tooltipText: 'Add New Vendor', prefixIcon: 'e-add', id: 'addBtn' },
                        { text: 'Edit', tooltipText: 'Edit Selected Vendor', prefixIcon: 'e-edit', id: 'editBtn' },
                        { text: 'Delete', tooltipText: 'Delete Selected Vendor', prefixIcon: 'e-delete', id: 'deleteBtn' }
                    ],
                    columns: [
                        { type: 'checkbox', width: 50 },
                        { field: 'id', isPrimaryKey: true, visible: false },
                        { field: 'number', headerText: 'Number', width: 120 },
                        { field: 'name', headerText: 'Name', width: 220 },
                        { field: 'vendorGroupName', headerText: 'Group', width: 150 },
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
                        // handle excel export
                        if (args.item && args.item.id && mainGrid.obj && mainGrid.obj.element) {
                            const excelId = mainGrid.obj.element.id + '_excelexport';
                            if (args.item.id === excelId) {
                                mainGrid.obj.excelExport();
                                return;
                            }
                        }

                        switch (args.item.id) {
                            case 'addBtn':
                                resetForm();
                                state.mainTitle = 'Add Vendor';
                                mainModal.obj.show();
                                break;

                            case 'editBtn': {
                                const selectedRow = mainGrid.obj.getSelectedRecords()[0];
                                if (selectedRow) {
                                    await loadForm(selectedRow);
                                    state.mainTitle = 'Edit Vendor';
                                    mainModal.obj.show();
                                }
                                break;
                            }

                            case 'deleteBtn': {
                                const rowToDelete = mainGrid.obj.getSelectedRecords()[0];
                                if (!rowToDelete) return;

                                const confirmResult = await Swal.fire({
                                    title: 'Delete Vendor?',
                                    text: `Are you sure you want to delete "${rowToDelete.name}"?`,
                                    icon: 'warning',
                                    showCancelButton: true,
                                    confirmButtonText: 'Yes, delete',
                                    cancelButtonText: 'Cancel'
                                });

                                if (confirmResult.isConfirmed) {
                                    try {
                                        const result = await AxiosManager.post('/Vendor/DeleteVendor', {
                                            id: rowToDelete.id
                                        });

                                        if (result.data.code === 200) {
                                            await refreshGrid();
                                            Swal.fire('Deleted!', 'Vendor has been deleted.', 'success');
                                        } else {
                                            Swal.fire('Error', result.data.message || 'Delete failed', 'error');
                                        }
                                    } catch (err) {
                                        console.error(err);
                                        Swal.fire('Error', 'Failed to delete vendor', 'error');
                                    }
                                }
                                break;
                            }
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
            state.vendorGroupId = null;
            state.countryId = null;
            state.governorateId = null;
            state.cityId = null;
            state.buildingNumber = '';
            state.floor = '';
            state.flatNumber = '';
            state.postalCode = '';
            state.street = '';
            state.mobile = '';

            if (vendorGroupDropDown.obj) vendorGroupDropDown.obj.value = null;
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

            nameText.refresh();
            numberText.refresh();
            trnText.refresh();
            buildingText.refresh();
            floorText.refresh();
            flatText.refresh();
            streetText.refresh();
            postalText.refresh();
            mobileText.refresh();
        };

        const loadForm = async (row) => {
            state.id = row.id || '';
            state.number = row.number || '';
            state.name = row.name || '';
            state.trn = row.trn || '';
            state.vendorGroupId = row.vendorGroupId || null;
            state.mobile = row.mobile || '';
            state.buildingNumber = row.buildingNumber || '';
            state.floor = row.floor || '';
            state.flatNumber = row.flatNumber || '';
            state.postalCode = row.postalCode || '';
            state.street = row.street || '';

            state.countryId = row.countryId || null;
            state.governorateId = row.governorateId || null;
            state.cityId = row.cityId || null;

            vendorGroupDropDown.refresh();

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

            nameText.refresh();
            numberText.refresh();
            trnText.refresh();
            buildingText.refresh();
            floorText.refresh();
            flatText.refresh();
            streetText.refresh();
            postalText.refresh();
            mobileText.refresh();
        };

        const refreshGrid = async () => {
            const res = await services.getMainData();
            state.mainData = res?.data?.content?.data || [];
            if (mainGrid.obj) mainGrid.obj.dataSource = state.mainData;
        };

        Vue.onMounted(async () => {
            try {
                await SecurityManager.authorizePage(['Vendors']);
                await SecurityManager.validateToken();

                const [mainRes, groupRes] = await Promise.all([
                    services.getMainData(),
                    services.getVendorGroups()
                ]);

                state.mainData = mainRes?.data?.content?.data || [];
                state.vendorGroups = groupRes?.data?.content?.data || [];

                mainGrid.create(state.mainData);
                vendorGroupDropDown.create();
                countryDropDown.create();
                governorateDropDown.create();
                cityDropDown.create();
                nameText.create();
                numberText.create();
                trnText.create();
                buildingText.create();
                floorText.create();
                flatText.create();
                streetText.create();
                postalText.create();
                mobileText.create();
                mainModal.create();
            } catch (e) {
                console.error('Page init error:', e);
            }
        });

        return {
            state,
            mainGridRef,
            mainModalRef,
            vendorGroupIdRef,
            countryRef,
            governorateRef,
            cityRef,
            nameRef,
            numberRef,
            trnRef,
            buildingRef,
            floorRef,
            flatRef,
            postalRef,
            streetRef,
            mobileRef,
            handler
        };
    }
};

Vue.createApp(App).mount('#app');