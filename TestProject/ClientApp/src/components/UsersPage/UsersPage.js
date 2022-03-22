import React, { useEffect, useState } from 'react'
import { shallowEqual, useDispatch, useSelector } from 'react-redux';
import List from '../ListComponents/List'
import { useTranslation } from 'react-i18next';
import { createUser, deleteUser, editUser, getUsers } from '../../actions/user';
import ModalWindow from '../ModalWindow/ModalWindow';
import { clearMessage } from '../../actions/message';
import { EmailInput, FieldInput, PasswordInput, SelectInput } from '../FormComponents';
import { FormGroup, Label } from 'reactstrap';

const UsersPage = () => {
    const { t } = useTranslation();
    const dispatch = useDispatch();
    const [modalAdd, setModalAdd] = useState(false);
    const [modalEdit, setModalEdit] = useState(false);
    const [model, setModel] = useState({ id: "", firstname: "", lastname: "", email: "", password: "", role: "" });
    const [validate, setValidate] = useState({ email: '' });

    const [roles] = useState([
        { id: 0, name: "User" },
        { id: 1, name: "Admin" }
    ]);

    const { users, message } = useSelector(state => ({
        users: state.user.users,
        message: state.message.message
    }), shallowEqual)

    useEffect(() => {
        dispatch(getUsers(t));
    }, [dispatch, t])

    const getRole = (role) => {
        switch (role) {
            case 0:
                return "User";
            case 1:
                return "Admin";
            default:
                return undefined;
        }
    }

    const clearFields = () => {
        setValidate({ email: '' });
        setModel({ id: "", firstname: "", lastname: "", email: "", password: "", role: "" });
    }

    const createRecord = () => {
        dispatch(createUser(model.lastname, model.firstname, model.role, model.email, model.password, t))
            .then(() => {
                setModalAdd(false);
                dispatch(clearMessage());
                clearFields();
            })
            .catch(() => { })
    }

    const getUserValues = (item) => {
        setModel({ ...item, password: "" });
        dispatch(clearMessage());
        setModalEdit(true);
    }

    const editRecord = () => {
        console.log(model);
        dispatch(editUser(model.id, model.lastname, model.firstname, parseInt(model.role), t))
            .then(() => {
                setModalEdit(false);
                dispatch(clearMessage());
                clearFields();
            })
            .catch(() => { })
    }

    const action = (item) => {
        return (
            <td>
                <button
                    onClick={() => { getUserValues(item) }}
                    style={{ marginRight: "3px" }}
                    className="btn btn-outline-success btn-sm float-left">
                    <i className="bi-pencil-square" />
                </button>
                <button
                    onClick={() => { dispatch(deleteUser(item.id, t)) }}
                    className="btn btn-outline-danger btn-sm float-left">
                    <i className="bi-trash" />
                </button>
            </td>
        )
    }

    return (
        <>
            <List
                name="users"
                records={users.map(user => ({ ...user, roleName: getRole(user.role) }))}
                k="id"
                columns={['firstname', 'lastname', 'email', 'roleName']}
                refreshRecords={() => { dispatch(getUsers(t)); }}
                createRecord={() => { clearFields(); dispatch(clearMessage()); setModalAdd(true); }}
                action={action}
            />
            <ModalWindow modal={modalAdd} deactiveModal={() => setModalAdd(false)} textHeader={t("Create")}
                textButton={t("Create")} method={createRecord} message={message}
            >
                <EmailInput name="email" validate={validate} setValidate={setValidate} model={model} setModel={setModel} />
                <FieldInput name="firstname" model={model} setModel={setModel} minLength={2} maxLength={30} />
                <FieldInput name="lastname" model={model} setModel={setModel} minLength={2} maxLength={30} />
                <PasswordInput name="password" model={model} setModel={setModel} />
                <SelectInput labelName="roleName" name="role" id="id" value="name" records={roles} model={model} setModel={setModel} />
            </ModalWindow>
            <ModalWindow modal={modalEdit} deactiveModal={() => setModalEdit(false)} textHeader={t("Edit")}
                method={editRecord} message={message} textButton={t("Edit")}
            >
                <FormGroup>
                    <Label>{t("email")}: {model.email}</Label>
                </FormGroup>
                <FieldInput name="firstname" model={model} setModel={setModel} minLength={2} maxLength={30} />
                <FieldInput name="lastname" model={model} setModel={setModel} minLength={2} maxLength={30} />
                <SelectInput labelName="roleName" name="role" id="id" value="name" records={roles} model={model} setModel={setModel} />
            </ModalWindow>
        </>
    );
};

export default UsersPage;