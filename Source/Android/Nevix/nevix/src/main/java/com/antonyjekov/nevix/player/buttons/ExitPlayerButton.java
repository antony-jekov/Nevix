package com.antonyjekov.nevix.player.buttons;

import android.graphics.Path;
import android.graphics.Rect;

import com.antonyjekov.nevix.constants.PlayerCommand;

/**
 * Created by Antony Jekov on 4/18/2014.
 */
public class ExitPlayerButton extends PointingDownButton {
    public ExitPlayerButton(Rect button, int strokeWidth, int padding) {
        super(button, strokeWidth, padding);
    }

    @Override
    protected void prepareShape(Path shape) {
        super.prepareShape(shape);

        int halfPadding = padding >> 1;
        int x = button.centerX() - halfPadding;

        shape.moveTo(x, button.centerY());
        shape.lineTo(x, button.top + padding);

        x = button.centerX() + halfPadding;

        shape.lineTo(x, button.top + padding);
        shape.lineTo(x, button.centerY());
        shape.close();
    }

    @Override
    public String command() {
        return PlayerCommand.EXIT_PLAYER;
    }
}
