package com.antonyjekov.nevix.activities;

import android.support.v7.app.ActionBarActivity;

import com.antonyjekov.nevix.App;

/**
 * Created by ajekov on 3/27/2014.
 */
public abstract class BaseActivity extends ActionBarActivity {

    protected final App application;

    protected BaseActivity () {
        this.application = (App) getApplication();
    }
}
