package com.antonyjekov.nevix.activities;

import android.content.Intent;
import android.content.pm.ActivityInfo;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.Toast;

import com.antonyjekov.nevix.R;
import com.antonyjekov.nevix.common.ContextManager;
import com.antonyjekov.nevix.common.HttpAsyncRequest;
import com.antonyjekov.nevix.common.PersistentManager;
import com.antonyjekov.nevix.common.contracts.OnFileSelected;
import com.antonyjekov.nevix.viewmodels.MediaFolderViewModel;
import com.google.gson.Gson;

import java.util.ArrayList;

public class BrowseActivity extends ActionBarActivity implements OnFileSelected {

    String lastServerUpdate;
    ContextManager db;
    PersistentManager persistentManager;

    public static final String BROWSED_FILE = "com.antonyjekov.nevix.browse.browsedFile";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_browse);

        //this.setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE);

        db = new ContextManager(this);
        String sessionKey = db.getSessionKey();
        persistentManager = new PersistentManager(sessionKey);

        final String lastLocalUpdate = db.getLastDatabaseUpdate();
        persistentManager.getLastMediaUpdateTime(new HttpAsyncRequest.OnResultCallBack() {
            @Override
            public void onResult(String result) {
                lastServerUpdate = result;
                if (result == null || !result.equals(lastLocalUpdate) ) {
                    pringMessage(getResources().getString(R.string.media_is_outdated));
                }
                else{

                }
            }
        });
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.browse, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        beginMediaSync();
        return true;
    }

    private void beginMediaSync() {
        pringMessage(getResources().getString(R.string.beginning_media_sync));

        persistentManager.getMedia(new HttpAsyncRequest.OnResultCallBack() {
            @Override
            public void onResult(String result) {
                db.storeMediaDatabase(result);
                db.setLastDatabaseUpdate(lastServerUpdate);
                // TODO: Update UI.
            }
        });
    }

    private void pringMessage(String message) {
        Toast.makeText(this, message, Toast.LENGTH_LONG).show();
    }

    @Override
    public void onFileSelected(int fileId) {
        Intent data = new Intent();
        data.putExtra(BROWSED_FILE, fileId);
        setResult(RESULT_OK, data);
        finish();
    }
}