package com.antonyjekov.nevix.fragments;

import android.app.AlertDialog;
import android.app.ProgressDialog;
import android.content.DialogInterface;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.antonyjekov.nevix.R;
import com.antonyjekov.nevix.activities.BaseActivity;
import com.antonyjekov.nevix.common.HttpAsyncRequest;
import com.antonyjekov.nevix.common.PersistentManager;
import com.antonyjekov.nevix.common.contracts.FailCallback;

public class AuthorizationFragment extends Fragment implements FailCallback {
    private Button loginBtn;
    private View resetBtn;
    private EditText email;
    private EditText pass;

    ProgressDialog progress;

    @Override
    public void onFail() {
        progress.hide();
        if (!checkInternetConnection()) {
            warnUser("No internet connection...");
        } else {
            new AlertDialog.Builder(getActivity())
                    .setTitle("Login failed")
                    .setMessage("The email or password do not match. Do you want to review the password you provided?\nWARRNING: It will be visible!")
                    .setIcon(android.R.drawable.ic_dialog_alert)
                    .setPositiveButton("Yes", new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            new AlertDialog.Builder(getActivity())
                                    .setTitle("The password you provided")
                                    .setMessage("'" + pass.getText().toString() + "'")
                                    .show();
                        }
                    })
                    .setNegativeButton("No", new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {

                        }
                    }).show();
        }
    }

    private boolean checkInternetConnection() {
        return ((BaseActivity) getActivity()).application().isNetworkConnected();
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
        persistent.wakeUpInternet();

        email = (EditText) loginView.findViewById(R.id.auth_email);
        pass = (EditText) loginView.findViewById(R.id.auth_pass);
        final FailCallback error = this;
        loginBtn = (Button) loginView.findViewById(R.id.auth_login_btn);
        loginBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                String emailText = email.getText().toString().trim();
                String passwordText = pass.getText().toString().trim();

                if (emailText.length() > 0 && passwordText.length() > 0) {
                    persistent.login(emailText, passwordText, callBack, error);
                    progress.show();
                } else {
                    warnUser("Please fill in all fields!");
                }
            }
        });

        resetBtn = loginView.findViewById(R.id.auth_reset_pass);
        resetBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                final String emailText = email.getText().toString().trim();

                if (emailText != null && emailText.length() > 0) {
                    new AlertDialog.Builder(getActivity())
                            .setTitle("Reset password")
                            .setMessage("Do you really want to reset your password?")
                            .setIcon(android.R.drawable.ic_dialog_alert)
                            .setPositiveButton(android.R.string.yes, new DialogInterface.OnClickListener() {

                                public void onClick(DialogInterface dialog, int whichButton) {
                                    persistent.resetPass(emailText, new HttpAsyncRequest.OnResultCallBack() {
                                                @Override
                                                public void onResult(String result) {
                                                    new AlertDialog.Builder(getActivity())
                                                            .setTitle("Reset completed")
                                                            .setMessage("Your reset request was submitted. Please check your inbox and follow the reset password link from there.")
                                                            .setIcon(android.R.drawable.ic_dialog_info)
                                                            .setPositiveButton("OK", new DialogInterface.OnClickListener() {
                                                                @Override
                                                                public void onClick(DialogInterface dialog, int which) {

                                                                }
                                                            })
                                                            .show();
                                                }
                                            },
                                            new FailCallback() {
                                                @Override
                                                public void onFail() {
                                                    warnUser("Your request could not be granted. Please check your internet connection.");
                                                }
                                            }
                                    );
                                }
                            })
                            .setNegativeButton(android.R.string.no, null).show();
                } else {
                    warnUser("Please provide the email you registered with.");
                }
            }
        });

        return loginView;
    }

    private void warnUser(String message) {
        Toast.makeText(getActivity(), message, Toast.LENGTH_LONG).show();
    }
}
