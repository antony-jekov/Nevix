package com.antonyjekov.nevix.fragments;

import android.app.ProgressDialog;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.antonyjekov.nevix.R;
import com.antonyjekov.nevix.common.HttpAsyncRequest;
import com.antonyjekov.nevix.common.PersistentManager;
import com.antonyjekov.nevix.common.contracts.FailCallback;

public class AuthorizationFragment extends Fragment implements FailCallback {
    private Button loginBtn;
    private EditText email;
    private EditText pass;

    ProgressDialog progress;

    @Override
    public void onFail() {
        progress.hide();
        warnUser("Bad username or password!");
    }

    HttpAsyncRequest.OnResultCallBack callBack;

    public AuthorizationFragment(HttpAsyncRequest.OnResultCallBack callBack) {
        this.callBack = callBack;
    }

    public static final String SESSION_KEY = "com.antonyjekov.nevix.login.sessionKey";

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        final View loginView = inflater.inflate(R.layout.fragment_authenticate, container, false);

        progress = new ProgressDialog(getActivity());
        progress.setTitle("Logging in");
        progress.setMessage("Connecting to server, please wait...");

        final PersistentManager persistent = new PersistentManager();

        email = (EditText) loginView.findViewById(R.id.auth_email);
        pass = (EditText) loginView.findViewById(R.id.auth_pass);
        final FailCallback error = this;
        loginBtn = (Button) loginView.findViewById(R.id.auth_login_btn);
        loginBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                String emailText = email.getText().toString();
                String passwordText = pass.getText().toString();

                if (emailText.length() > 0 && passwordText.length() > 0) {
                    persistent.login(emailText, passwordText, callBack, error);
                    progress.show();
                } else {
                    warnUser("Please fill in all fields!");
                }
            }
        });

        return loginView;
    }

    private void warnUser(String message) {
        Toast.makeText(getActivity(), message, Toast.LENGTH_LONG).show();
    }
}
