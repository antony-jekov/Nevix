package com.antonyjekov.nevix.activities.browse;

import android.app.AlertDialog;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.ScrollView;
import android.widget.Toast;

import com.antonyjekov.nevix.R;
import com.antonyjekov.nevix.activities.AuthenticateActivity;
import com.antonyjekov.nevix.activities.BaseActivity;
import com.antonyjekov.nevix.common.ContextManager;
import com.antonyjekov.nevix.common.FolderAdapter;
import com.antonyjekov.nevix.common.HttpAsyncRequest;
import com.antonyjekov.nevix.common.MediaItem;
import com.antonyjekov.nevix.common.PersisterManager;
import com.antonyjekov.nevix.common.contracts.FailCallback;
import com.antonyjekov.nevix.common.contracts.OnFileSelected;
import com.antonyjekov.nevix.viewmodels.MediaFileViewModel;
import com.antonyjekov.nevix.viewmodels.MediaFolderViewModel;
import com.google.gson.Gson;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.Stack;

//import com.startapp.android.publish.StartAppAd;

public class BrowseActivity extends BaseActivity implements OnFileSelected {

    public static final String BROWSED_FILE_NAME = "com.antonyjekov.nevix.browse.browsedFile.name";
    private Button backBtn;
    private Button forwardBtn;
    //Random random;

    Stack<MediaFolderViewModel> backQueue;
    Stack<MediaFolderViewModel> forwardQueue;

    private LinearLayout list;
    private String lastServerUpdate;
    private ContextManager db;
    private PersisterManager persisterManager;
    private ScrollView mediaList;
    private MediaFolderViewModel rootFolder;
    private MediaFileViewModel mediaFile;

    ProgressDialog progressDialog;
    //private StartAppAd startAppAd = new StartAppAd(this);

    public static final String BROWSED_FILE = "com.antonyjekov.nevix.browse.browsedFile";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        this.mediaFile = new MediaFileViewModel();
        Log.d("CLASS SSS", this.mediaFile.getClass().getName());
        Log.d("PROGRESS-PRE", "1");
        progressDialog = new ProgressDialog(this);
        progressDialog.setTitle(getResources().getString(R.string.connecting_server));
        progressDialog.setMessage(getResources().getString(R.string.syncing));

        backQueue = new Stack<MediaFolderViewModel>();
        forwardQueue = new Stack<MediaFolderViewModel>();
        super.onCreate(savedInstanceState);
        //StartAppAd.init(this, "104083607", "204814165");
        setContentView(R.layout.activity_browse);
        db = new ContextManager(this);
        String media = db.getMediaDatabase();
        getSupportActionBar().hide();
        Log.d("PROGRESS-PRE", "2");
        rootFolder = loadMedia(media);
        Log.d("CLASS", this.rootFolder.getClass().getName());
        Log.d("PROGRESS-PRE", "3");
        final Context cont = this;
        this.mediaList = (ScrollView) findViewById(R.id.media_list);
        Log.d("PROGRESS-PRE", "4");
        list = (LinearLayout) findViewById(R.id.jekozo);
        Log.d("PROGRESS-PRE", "5");

        String sessionKey = db.getSessionKey();
        persisterManager = new PersisterManager(sessionKey);
        Log.d("PROGRESS-PRE", "6");
        openRootFolder();
        Log.d("PROGRESS-PRE", "7");

        final String lastLocalUpdate = db.getLastDatabaseUpdate();
        persisterManager.getLastMediaUpdateTime(new HttpAsyncRequest.OnResultCallBack() {
            @Override
            public void onResult(String result) {
                lastServerUpdate = result;
                if (result == null || !result.equals(lastLocalUpdate)) {
                    beginMediaSync();
                }
            }
        }, new FailCallback() {
            @Override
            public void onFail() {
                showMessage(getResources().getString(R.string.could_not_last_update));
            }
        });

        Log.d("PROGRESS-PRE", "8");

        backBtn = (Button) findViewById(R.id.back_btn);
        backBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                forwardQueue.push(rootFolder);
                rootFolder = backQueue.pop();
                openRootFolder();
                updateButtons();
            }
        });

        Log.d("PROGRESS-PRE", "9");

        forwardBtn = (Button) findViewById(R.id.forward_btn);
        forwardBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                backQueue.push(rootFolder);
                rootFolder = forwardQueue.pop();
                openRootFolder();
                updateButtons();
            }
        });

        View syncBtn = findViewById(R.id.sync_btn);
        syncBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                beginMediaSync();
            }
        });
        Log.d("PROGRESS-PRE", "10");
        backBtn.setEnabled(false);
        forwardBtn.setEnabled(false);
        final Context thisContext = this;
        Log.d("PROGRESS-PRE", "11");
        View logOffBtn = findViewById(R.id.logoff_btn);
        logOffBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                new AlertDialog.Builder(thisContext)
                        .setTitle(getResources().getString(R.string.loggoff))
                        .setMessage(getResources().getString(R.string.realy_quit_message))
                        .setIcon(android.R.drawable.ic_dialog_alert)
                        .setPositiveButton(android.R.string.yes, new DialogInterface.OnClickListener() {

                            public void onClick(DialogInterface dialog, int whichButton) {
                                logoff(thisContext);
                            }
                        })
                        .setNegativeButton(android.R.string.no, null).show();
            }
        });

        Log.d("PROGRESS-PRE", "12");

        //this.random = new Random();
    }

    private void logoff(Context thisContext) {
        db.setSessionKey(ContextManager.EMPTY_SESSION_KEY);
        Intent logoff = new Intent(thisContext, AuthenticateActivity.class);
        startActivity(logoff);
        finish();
    }

    private void updateButtons() {
        backBtn.setEnabled(backQueue.size() > 0);
        forwardBtn.setEnabled(forwardQueue.size() > 0);
    }

    private void beginMediaSync() {
        Log.d("PROGRESS", "1");
        progressDialog.show();
        persisterManager.getMedia(new HttpAsyncRequest.OnResultCallBack() {
            @Override
            public void onResult(String result) {
                Log.d("PROGRESS", "2");
                progressDialog.hide();
                db.storeMediaDatabase(result);
                Log.d("PROGRESS", "3");
                db.setLastDatabaseUpdate(lastServerUpdate);
                Log.d("PROGRESS", "4");
                rootFolder = loadMedia(result);
                Log.d("PROGRESS", "5");
                openRootFolder();
                backQueue.clear();
                forwardQueue.clear();
                backBtn.setEnabled(false);
                forwardBtn.setEnabled(false);
            }
        }, new FailCallback() {
            @Override
            public void onFail() {
                progressDialog.hide();
                showMessage(getResources().getString(R.string.media_sing_fail));
            }
        });
    }

    @Override
    public void onFileSelected(int fileId, String fileName) {

        /*int randomNumber = this.random.nextInt(101);

        if (randomNumber <= 40) {
            startAppAd.showAd();
            startAppAd.loadAd();
        }*/

        Intent data = new Intent();
        data.putExtra(BROWSED_FILE, fileId);
        data.putExtra(BROWSED_FILE_NAME, fileName);
        setResult(RESULT_OK, data);
        finish();
    }

    private MediaFolderViewModel loadMedia(String media) {
        ArrayList<MediaFolderViewModel> folders = new ArrayList<MediaFolderViewModel>();
        StringBuilder stringBuilder = new StringBuilder();
        int index = 0;
        boolean inBrackets = false;
        int curlyBracketsCount = 0;
        char[] mediaChars = media.toCharArray();
        int len = mediaChars.length;

        MediaFolderViewModel currentFolder;
        char currentChar;
        while (index < len) {
            currentChar = mediaChars[index++];

            if (currentChar == '"') {
                inBrackets = !inBrackets;
            } else if (!inBrackets && currentChar == '{') {
                curlyBracketsCount++;
            } else if (!inBrackets && currentChar == '}') {
                curlyBracketsCount--;
            }

            if (curlyBracketsCount > 0) {
                stringBuilder.append(currentChar);
            } else if (curlyBracketsCount == 0 && stringBuilder.length() > 0) {
                stringBuilder.append('}');
                currentFolder = new Gson().fromJson(stringBuilder.toString(), MediaFolderViewModel.class);
                folders.add(currentFolder);
                stringBuilder.setLength(0);
            }
        }

        MediaFolderViewModel root;
        if (folders.size() == 1) {
            root = folders.get(0);
        } else {
            root = new MediaFolderViewModel();
            root.setName("Root");
            root.setFolders(folders);
        }

        return root;
    }

    private void openRootFolder() {
        Log.d("ORF", "1");
        this.mediaList.fullScroll(ScrollView.FOCUS_UP);
        Log.d("ORF", "2");
        list.removeAllViews();
        Log.d("ORF", "3");
        final ArrayList<MediaItem> contents = new ArrayList<MediaItem>();
        Log.d("ORF", "4");
        int folderIcon = R.drawable.media_folder2;
        int movieIcon = R.drawable.media_icon;
        int count = 0;

        Log.d("ORF", "5");
        for (MediaFolderViewModel folder : rootFolder.getFolders()) {
            contents.add(new MediaItem(folder.getName(), folderIcon, count++));
        }
        Log.d("ORF", "6");
        Class<?> cls = rootFolder.getFiles().get(0).getClass();
        Log.d("CAST", cls.getName());

        for (MediaFileViewModel file : rootFolder.getFiles()) {
            contents.add(new MediaItem(file.getName(), movieIcon, count++));
        }

        Log.d("ORF", "7");
        ArrayAdapter adapter = new FolderAdapter(this, R.layout.media_item, contents.toArray());
        Log.d("ORF", "8");
        for (int i = 0; i < count; i++) {
            View mediaItem = adapter.getView(i, null, this.mediaList);
            mediaItem.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    for (int view = 0; view < list.getChildCount(); view++)
                        if (list.getChildAt(view) == v)
                            onListItemClick(view);
                }
            });
            list.addView(mediaItem);
        }
        Log.d("ORF", "9");
    }

    public void onListItemClick(int position) {
        forwardQueue.clear();
        int index = position;
        int foldersCount = rootFolder.getFolders().size();

        if (index >= foldersCount) {
            index -= foldersCount;
            this.onFileSelected(rootFolder.getFiles().get(index).getId(), rootFolder.getFiles().get(index).getName());
        } else {
            MediaFolderViewModel selectedFolder = rootFolder.getFolders().get(index);
            try {
                backQueue.push((MediaFolderViewModel) rootFolder.clone());
            } catch (CloneNotSupportedException e) {
                e.printStackTrace();
            }
            rootFolder = selectedFolder;
            openRootFolder();
        }
        updateButtons();
    }

    /*@Override
    protected void onResume() {
        super.onResume();
        this.startAppAd.onResume();
    }*/

    private void showMessage(String message) {
        Toast.makeText(this, message, Toast.LENGTH_LONG).show();
    }
}