import React, { useEffect, useState } from 'react'
import { shallowEqual, useDispatch, useSelector } from 'react-redux';
import List from '../ListComponents/List'
import { useTranslation } from 'react-i18next';
import { createUser, deleteUser, getRoles, getUsers } from '../../actions/user';
import ModalWindow from '../ModalWindow/ModalWindow';
import { clearMessage } from '../../actions/message';
import { EmailInput, FieldInput, PasswordInput, SelectInput } from '../FormComponents';

const UsersPage = () => {
    const { t } = useTranslation();
    const dispatch = useDispatch();
    const [modalAdd, setModalAdd] = useState(false);
    const [model, setModel] = useState({ id: "", firstname: "", lastname: "", email: "", password: "", role: "" });
    const [validate, setValidate] = useState({ email: '' });

    const { users, roles, message } = useSelector(state => ({
        users: state.user.users,
        roles: state.user.roles,
        message: state.message.message
    }), shallowEqual)

    useEffect(() => {
        dispatch(getUsers(t));
        dispatch(getRoles(t));
    }, [dispatch, t])

    const clearFields = () => {
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

    const action = (item) => {
        return (
            <td>
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
                records={users}
                k="id"
                columns={['firstname', 'lastname', 'email', 'role']}
                refreshRecords={() => { dispatch(getUsers(t)); dispatch(getRoles(t)); }}
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
                <SelectInput name="role" id="id" value="name" records={roles} model={model} setModel={setModel} />
            </ModalWindow>
        </>
    );
};

export default UsersPage;