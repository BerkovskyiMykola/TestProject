import React from 'react';
import { useTranslation } from "react-i18next";
import { validateEmail } from "../../validation/validation"
import handleChange from "../../utils/handleChange";

import {
    FormFeedback,
    FormGroup,
    Input,
    Label
} from 'reactstrap';

export const EmailInput = ({ name, validate, setValidate, model, setModel }) => {

    const { t } = useTranslation();

    return (
        <FormGroup>
            <Label for={name}>{t(name)}</Label>
            <Input
                type="email"
                name={name}
                id={name}
                placeholder="example@example.com"
                required
                valid={validate[name] === "has-success"}
                invalid={validate[name] === "has-danger"}
                value={model[name]}
                onChange={(e) => {
                    validateEmail(validate, setValidate)(e);
                    handleChange(model, setModel)(e);
                }}
            />
            <FormFeedback>
                Uh oh! Looks like there is an issue with your email. Please input
                a correct email.
            </FormFeedback>
            <FormFeedback valid>
                That's a tasty looking email you've got there.
            </FormFeedback>
        </FormGroup>
    );
}

export const PasswordInput = ({ name, model, setModel }) => {

    const { t } = useTranslation();

    return (
        <FormGroup>
            <Label for={name}>{t(name)}</Label>
            <Input
                type="password"
                name={name}
                id={name}
                placeholder="********"
                required
                value={model[name]}
                onChange={handleChange(model, setModel)}
                minLength={8}
                maxLength={18}
            />
        </FormGroup>
    );
}

export const FieldInput = ({ name, model, setModel, minLength, maxLength, required = true }) => {

    const { t } = useTranslation();

    return (
        <FormGroup>
            <Label for={name}>{t(name)}</Label>
            <Input
                type="text"
                name={name}
                id={name}
                required={required}
                value={model[name]}
                onChange={handleChange(model, setModel)}
                minLength={minLength}
                maxLength={maxLength}
            />
        </FormGroup>
    );
}

export const SelectInput = ({ name, id, value, records, model, setModel, required = true }) => {

    const { t } = useTranslation();

    return (
        <FormGroup>
            <Label for={name}>{t(name)}</Label>
            <Input
                type="select"
                name={name}
                id={name}
                required={required}
                value={model[name]}
                onChange={handleChange(model, setModel)}
            >
                <option value="">{t("defaultOption")}</option>
                {records.map(x => <option key={x[id]} value={x[id]}>{x[value]}</option>)}
            </Input>
        </FormGroup>
    );
}