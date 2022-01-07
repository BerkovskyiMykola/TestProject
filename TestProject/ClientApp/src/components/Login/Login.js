import React, { useState } from "react";
import { useTranslation } from "react-i18next";

import { shallowEqual, useDispatch, useSelector } from "react-redux";
import { Redirect } from "react-router-dom";
import { login } from "../../actions/auth";

import { Form, EmailInput, PasswordInput } from "../FormComponents";

import {
    Button,
    FormGroup,
} from 'reactstrap';

export default function Login() {
    const { t } = useTranslation();
    const [model, setModel] = useState({ email: "", password: "" });
    const [validate, setValidate] = useState({ email: '' });

    const dispatch = useDispatch();

    const { message, isLoggedIn } = useSelector(state => ({
        message: state.message.message,
        isLoggedIn: state.auth.isLoggedIn
    }), shallowEqual)

    const submitForm = (e) => {
        e.preventDefault();

        dispatch(login(model.email, model.password))
    }

    if (isLoggedIn) {
        return <Redirect to="/profile" />;
    }

    return (
        <div className="col-md-12">
            <div className="card card-container">
                <Form handleSubmit={submitForm} message={message}>
                    <EmailInput name="email" validate={validate} setValidate={setValidate} model={model} setModel={setModel} />
                    <PasswordInput name="password" model={model} setModel={setModel} />
                    <FormGroup>
                        <Button block color="primary">{t("Login")}</Button>
                    </FormGroup>
                </Form>
            </div>
        </div>
    )
}