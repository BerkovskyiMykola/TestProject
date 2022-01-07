import React, { useState } from "react";
import { useTranslation } from "react-i18next";

import { shallowEqual, useDispatch, useSelector } from "react-redux";
import { Redirect } from "react-router-dom";
import { register } from "../../actions/auth";

import {
    Button,
    FormGroup,
} from 'reactstrap';
import { Form, EmailInput, PasswordInput, FieldInput } from "../FormComponents";

export default function Register() {
    const { t } = useTranslation();
    const [model, setModel] = useState({ firstname: '', lastname: '', email: '', password: '' });
    const [validate, setValidate] = useState({ email: '' });

    const dispatch = useDispatch();

    const { message, isLoggedIn } = useSelector(state => ({
        message: state.message.message,
        isLoggedIn: state.auth.isLoggedIn
    }), shallowEqual)

    const submitForm = (e) => {
        e.preventDefault();

        dispatch(register(model.lastname, model.firstname, model.email, model.password))
    }

    if (isLoggedIn) {
        return <Redirect to="/profile" />;
    }

    return (
        <div className="col-md-12">
            <div className="card card-container">
                <Form handleSubmit={submitForm} message={message}>
                    <EmailInput name="email" validate={validate} setValidate={setValidate} model={model} setModel={setModel} />
                    <FieldInput name="firstname" model={model} setModel={setModel} minLength={2} maxLength={30} />
                    <FieldInput name="lastname" model={model} setModel={setModel} minLength={2} maxLength={30} />
                    <PasswordInput name="password" model={model} setModel={setModel} />
                    <FormGroup>
                        <Button block color="primary">{t("SignUp")}</Button>
                    </FormGroup>
                </Form>
            </div>
        </div>
    );
}