package com.antonyjekov.nevix.player.buttons;

import android.graphics.Path;
import android.graphics.Rect;

import com.antonyjekov.nevix.constants.PlayerCommand;

/**
 * Created by Antony Jekov on 4/1/2014.
 */
public class PauseButton extends SphericalButton {
    public PauseButton(Rect button) {
        super(button);
    }

    @Override
    protected void prepareShape(Path shape) {
        super.prepareShape(shape);

        int doubleWidth = strokeWidth << 1;

        shape.moveTo(button.centerX() - doubleWidth, button.top + padding);
        shape.lineTo(button.centerX() - doubleWidth, button.bottom - padding);

        shape.moveTo(button.centerX() + doubleWidth, button.top + padding);
        shape.lineTo(button.centerX() + doubleWidth, button.bottom - padding);
    }

    @Override
    public String command() {
        return PlayerCommand.PAUSE_CMD;
    }
}
