package com.antonyjekov.nevix.player;

import android.content.Context;
import android.graphics.Rect;

import com.antonyjekov.nevix.activities.MainActivity;
import com.antonyjekov.nevix.common.PusherManager;
import com.antonyjekov.nevix.player.buttons.BringToFrontButton;
import com.antonyjekov.nevix.player.buttons.BrowseButton;
import com.antonyjekov.nevix.player.buttons.ExitPlayerButton;
import com.antonyjekov.nevix.player.buttons.FastForwardButton;
import com.antonyjekov.nevix.player.buttons.FullScreenButton;
import com.antonyjekov.nevix.player.buttons.MuteButton;
import com.antonyjekov.nevix.player.buttons.NextButton;
import com.antonyjekov.nevix.player.buttons.PauseButton;
import com.antonyjekov.nevix.player.buttons.PlayButton;
import com.antonyjekov.nevix.player.buttons.PowerDownButton;
import com.antonyjekov.nevix.player.buttons.PreviousButton;
import com.antonyjekov.nevix.player.buttons.RewindButton;
import com.antonyjekov.nevix.player.buttons.StopButton;
import com.antonyjekov.nevix.player.buttons.SystemVolumeDownButton;
import com.antonyjekov.nevix.player.buttons.SystemVolumeUpButton;
import com.antonyjekov.nevix.player.buttons.VolumeDownButton;
import com.antonyjekov.nevix.player.buttons.VolumeUpButton;

/**
 * Created by Antony Jekov on 3/23/2014.
 */
public class BSPlayer extends Player {

    public BSPlayer(Context context, PusherManager pusher, MainActivity owner, InteractionIndicator indicator) {
        super(context, pusher, owner, indicator);
    }

    @Override
    protected void arrangeButtons() {
        int halfWidth = width >> 1;
        int halfHeight = height >> 1;
        int strokeWidth = (int) (width * .007);
        int buttonPadding;
        float paddingFactor = .2f;

        int buttonHalfSize = ((int) (width * .15)) >> 1;
        buttonPadding = (int) ((buttonHalfSize << 1) * paddingFactor);
        Rect playBtn = new Rect(halfWidth - buttonHalfSize, halfHeight - buttonHalfSize, halfWidth + buttonHalfSize, halfHeight + buttonHalfSize);
        buttons.add(new PlayButton(playBtn, strokeWidth, buttonPadding));

        int buttonSize = ((int) (width * .12));
        buttonPadding = (int) (buttonSize * paddingFactor);

        Rect ffBtn = rightTo(playBtn, buttonSize);
        buttons.add(new FastForwardButton(ffBtn, strokeWidth, buttonPadding));

        Rect nextBtn = bellowOf(ffBtn, buttonSize);
        buttons.add(new NextButton(nextBtn, strokeWidth, buttonPadding));

        Rect rwBtn = leftTo(playBtn, buttonSize);
        buttons.add(new RewindButton(rwBtn, strokeWidth, buttonPadding));

        Rect prevBtn = bellowOf(rwBtn, buttonSize);
        buttons.add(new PreviousButton(prevBtn, strokeWidth, buttonPadding));

        Rect pauseBtn = onTopOf(playBtn, buttonSize);
        buttons.add(new PauseButton(pauseBtn, strokeWidth, buttonPadding));

        Rect stopBtn = bellowOf(playBtn, buttonSize);
        buttons.add(new StopButton(stopBtn, strokeWidth, buttonPadding));

        buttonSize = (int) (width * .08);
        buttonPadding = (int) (buttonSize * paddingFactor);

        Rect powerBtn = new Rect(buttonMargin, buttonMargin, buttonMargin + buttonSize, buttonMargin + buttonSize);
        buttons.add(new PowerDownButton(powerBtn, strokeWidth, buttonPadding));

        Rect fullBtn = new Rect(width - (buttonSize + buttonMargin), buttonMargin, width - buttonMargin, buttonMargin + buttonSize);
        buttons.add(new FullScreenButton(fullBtn, strokeWidth, buttonPadding));

        Rect volUpBtn = bellowOf(fullBtn, buttonSize);
        buttons.add(new VolumeUpButton(volUpBtn, strokeWidth, buttonPadding));

        Rect volumeDownBtn = bellowOf(volUpBtn, buttonSize);
        buttons.add(new VolumeDownButton(volumeDownBtn, strokeWidth, buttonPadding));

        Rect sysVolUp = bellowOf(powerBtn, buttonSize);//new Rect(buttonMargin, buttonMargin, buttonMargin + buttonSize, buttonMargin + buttonSize);
        buttons.add(new SystemVolumeUpButton(sysVolUp, strokeWidth, buttonPadding));

        Rect sysVolDown = bellowOf(sysVolUp, buttonSize);
        buttons.add(new SystemVolumeDownButton(sysVolDown, strokeWidth, buttonPadding));

        Rect browseBtn = leftTo(fullBtn, buttonSize);
        buttons.add(new BrowseButton(browseBtn, strokeWidth, buttonPadding));

        Rect muteBtn = rightTo(powerBtn, buttonSize);
        buttons.add(new MuteButton(muteBtn, strokeWidth, buttonPadding));

        Rect bringUpBtn = bellowOf(volumeDownBtn, buttonSize);
        buttons.add(new BringToFrontButton(bringUpBtn, strokeWidth, buttonPadding));

        Rect exitBtn = bellowOf(sysVolDown, buttonSize);
        buttons.add(new ExitPlayerButton(exitBtn, strokeWidth, buttonPadding));
    }
}
